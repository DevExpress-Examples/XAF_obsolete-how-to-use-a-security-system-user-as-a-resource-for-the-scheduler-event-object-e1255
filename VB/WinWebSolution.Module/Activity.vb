Imports Microsoft.VisualBasic
Imports System
Imports System.Xml
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp
Imports System.ComponentModel
Imports DevExpress.Xpo.Metadata
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.Validation
Imports DevExpress.Persistent.Base.General

Namespace WinWebSolution.Module
	<DefaultClassOptions> _
	Public Class Activity
		Inherits BasePersistentObject
		Implements IEvent, IRecurrentEvent
		Private _AllDay As Boolean
		Private _Description As String
		Private _StartOn As DateTime
		Private _EndOn As DateTime
		Private _Label As Integer
		Private _Location As String
		Private _Status As Integer
		Private _Subject As String
		Private _Type As Integer
		Private _RecurrenceInfoXml As String
		<Persistent("ResourceIds"), Size(SizeAttribute.Unlimited)> _
		Private _EmployeeIds As String
		<Persistent("RecurrencePattern")> _
		Private _RecurrencePattern As Activity
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Public Overrides Sub AfterConstruction()
			MyBase.AfterConstruction()
			StartOn = DateTime.Now
			EndOn = StartOn.AddHours(1)
			Employees.Add(Session.GetObjectByKey(Of Employee)(SecuritySystem.CurrentUserId))
		End Sub
		<Association("Activity-Employees", UseAssociationNameAsIntermediateTableName := True)> _
		Public ReadOnly Property Employees() As XPCollection(Of Employee)
			Get
				Return GetCollection(Of Employee)("Employees")
			End Get
		End Property
		Protected Overrides Function CreateCollection(Of T)(ByVal [property] As XPMemberInfo) As XPCollection(Of T)
			Dim result As XPCollection(Of T) = MyBase.CreateCollection(Of T)([property])
			If [property].Name = "Employees" Then
				AddHandler result.ListChanged, AddressOf Employees_ListChanged
			End If
			Return result
		End Function
		Public Sub UpdateEmployeeIds()
			_EmployeeIds = String.Empty
			For Each activityUser As Employee In Employees
				_EmployeeIds &= String.Format("<ResourceId Type=""{0}"" Value=""{1}"" />", activityUser.Id.GetType().FullName, activityUser.Id)
			Next activityUser
			_EmployeeIds = String.Format("<ResourceIds>{0}</ResourceIds>", _EmployeeIds)
		End Sub
		Private Sub UpdateEmployees()
			Employees.SuspendChangedEvents()
			Try
				Do While Employees.Count > 0
					Employees.Remove(Employees(0))
				Loop
				If (Not String.IsNullOrEmpty(_EmployeeIds)) Then
					Dim xmlDocument As New XmlDocument()
					xmlDocument.LoadXml(_EmployeeIds)
					For Each xmlNode As XmlNode In xmlDocument.DocumentElement.ChildNodes
						Dim activityUser As Employee = Session.GetObjectByKey(Of Employee)(New Guid(xmlNode.Attributes("Value").Value))
						If activityUser IsNot Nothing Then
							Employees.Add(activityUser)
						End If
					Next xmlNode
				End If
			Finally
				Employees.ResumeChangedEvents()
			End Try
		End Sub
		Private Sub Employees_ListChanged(ByVal sender As Object, ByVal e As ListChangedEventArgs)
			If e.ListChangedType = ListChangedType.ItemAdded OrElse e.ListChangedType = ListChangedType.ItemDeleted Then
				UpdateEmployeeIds()
				OnChanged("ResourceId")
			End If
		End Sub
		Protected Overrides Sub OnLoaded()
			MyBase.OnLoaded()
			If Employees.IsLoaded AndAlso (Not Session.IsNewObject(Me)) Then
				Employees.Reload()
			End If
		End Sub
		<Browsable(False), RuleFromBoolProperty("EventIntervalValid", DefaultContexts.Save, "The start date must be less than the end date", UsedProperties := "StartOn, EndOn")> _
		Public ReadOnly Property IsIntervalValid() As Boolean
			Get
				Return AllDay OrElse StartOn <= EndOn
			End Get
		End Property
		#Region "IEvent Members"
		Public Property AllDay() As Boolean Implements IEvent.AllDay
			Get
				Return _AllDay
			End Get
			Set(ByVal value As Boolean)
				SetPropertyValue("AllDay", _AllDay, value)
			End Set
		End Property
		<Browsable(False)> _
		Public ReadOnly Property AppointmentId() As Object Implements IEvent.AppointmentId
			Get
				Return Oid.ToString()
			End Get
		End Property
		<Size(SizeAttribute.Unlimited)> _
		Public Property Description() As String Implements IEvent.Description
			Get
				Return _Description
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Description", _Description, value)
			End Set
		End Property
		Public Property Label() As Integer Implements IEvent.Label
			Get
				Return _Label
			End Get
			Set(ByVal value As Integer)
				SetPropertyValue("Label", _Label, value)
			End Set
		End Property
		Public Property Location() As String Implements IEvent.Location
			Get
				Return _Location
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Location", _Location, value)
			End Set
		End Property
		<PersistentAlias("_EmployeeIds"), Browsable(False)> _
		Public Property ResourceId() As String Implements IEvent.ResourceId
			Get
				If _EmployeeIds Is Nothing Then
					UpdateEmployeeIds()
				End If
				Return _EmployeeIds
			End Get
			Set(ByVal value As String)
				If _EmployeeIds <> value AndAlso value IsNot Nothing Then
					_EmployeeIds = value
					UpdateEmployees()
				End If
			End Set
		End Property
		<Indexed, Custom("DisplayFormat", "{0:G}"), Custom("EditMask", "G")> _
		Public Property StartOn() As DateTime Implements IEvent.StartOn
			Get
				Return _StartOn
			End Get
			Set(ByVal value As DateTime)
				SetPropertyValue("StartOn", _StartOn, value)
			End Set
		End Property
		<Indexed, Custom("DisplayFormat", "{0:G}"), Custom("EditMask", "G")> _
		Public Property EndOn() As DateTime Implements IEvent.EndOn
			Get
				Return _EndOn
			End Get
			Set(ByVal value As DateTime)
				SetPropertyValue("EndOn", _EndOn, value)
			End Set
		End Property
		Public Property Status() As Integer Implements IEvent.Status
			Get
				Return _Status
			End Get
			Set(ByVal value As Integer)
				SetPropertyValue("Status", _Status, value)
			End Set
		End Property
		<Size(250)> _
		Public Property Subject() As String Implements IEvent.Subject
			Get
				Return _Subject
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Subject", _Subject, value)
			End Set
		End Property
		<Browsable(False)> _
		Public Property Type() As Integer Implements IEvent.Type
			Get
				Return _Type
			End Get
			Set(ByVal value As Integer)
				SetPropertyValue("Type", _Type, value)
			End Set
		End Property
		#End Region

		#Region "IRecurrentEvent Members"
		<DevExpress.Xpo.DisplayName("Recurrence"), Size(SizeAttribute.Unlimited)> _
		Public Property RecurrenceInfoXml() As String Implements ISupportRecurrences.RecurrenceInfoXml
			Get
				Return _RecurrenceInfoXml
			End Get
			Set(ByVal value As String)
				SetPropertyValue("RecurrenceInfoXml", _RecurrenceInfoXml, value)
			End Set
		End Property
		<Browsable(False), PersistentAlias("_RecurrencePattern")> _
		Public Property RecurrencePattern() As IRecurrentEvent Implements ISupportRecurrences.RecurrencePattern
			Get
				Return _RecurrencePattern
			End Get
			Set(ByVal value As IRecurrentEvent)
				SetPropertyValue("RecurrencePattern", _RecurrencePattern, TryCast(value, Activity))
			End Set
		End Property
		#End Region

	End Class
End Namespace