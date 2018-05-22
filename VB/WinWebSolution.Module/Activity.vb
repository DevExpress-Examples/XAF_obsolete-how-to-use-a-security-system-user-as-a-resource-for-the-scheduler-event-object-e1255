Imports Microsoft.VisualBasic
Imports System
Imports System.Xml
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp
Imports System.ComponentModel
Imports DevExpress.Xpo.Metadata
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Base.General

Namespace WinWebSolution.Module
	<DefaultClassOptions> _
	Public Class Activity
		Inherits BaseObject
		Implements IEvent, ISupportRecurrences
		Private _AllDay As Boolean
		Private _Description As String
		Private _StartOn As DateTime
		Private _EndOn As DateTime
		Private _Label As Integer
		Private _Location As String
		Private _Status As Integer
		Private _Subject As String
		Private _Type As Integer
		<Persistent("ResourceIds"), Size(SizeAttribute.Unlimited)> _
		Private _EmployeeIds As String
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Public Property AllDay() As Boolean Implements IEvent.AllDay
			Get
				Return Me._AllDay
			End Get

			Set(ByVal value As Boolean)
				SetPropertyValue("AllDay", Me._AllDay, value)
			End Set
		End Property
		<NonPersistent, Browsable(False)> _
		Public ReadOnly Property AppointmentId() As String Implements IEvent.AppointmentId
			Get
				Return Me.Oid.ToString()
			End Get
		End Property
		<Size(SizeAttribute.Unlimited)> _
		Public Property Description() As String Implements IEvent.Description
			Get
				Return Me._Description
			End Get

			Set(ByVal value As String)
				SetPropertyValue("Description", Me._Description, value)
			End Set
		End Property
		Public Property Label() As Integer Implements IEvent.Label
			Get
				Return Me._Label
			End Get

			Set(ByVal value As Integer)
				SetPropertyValue("Label", Me._Label, value)
			End Set
		End Property
		Public Property Location() As String Implements IEvent.Location
			Get
				Return Me._Location
			End Get

			Set(ByVal value As String)
				SetPropertyValue("Location", Me._Location, value)
			End Set
		End Property
		<PersistentAlias("_EmployeeIds"), Browsable(False)> _
		Public Property ResourceId() As String Implements IEvent.ResourceId
			Get
				If Me._EmployeeIds Is Nothing Then
					Me.UpdateEmployeeIds()
				End If
				Return Me._EmployeeIds
			End Get
			Set(ByVal value As String)
				If _EmployeeIds <> value AndAlso value IsNot Nothing Then
					Me._EmployeeIds = value
					Me.UpdateEmployees()
				End If
			End Set
		End Property
		<Indexed> _
		Public Property StartOn() As DateTime Implements IEvent.StartOn
			Get
				Return Me._StartOn
			End Get

			Set(ByVal value As DateTime)
				SetPropertyValue("StartOn", Me._StartOn, value)
			End Set
		End Property
		<Indexed> _
		Public Property EndOn() As DateTime Implements IEvent.EndOn
			Get
				Return Me._EndOn
			End Get

			Set(ByVal value As DateTime)
				SetPropertyValue("EndOn", Me._EndOn, value)
			End Set
		End Property
		Public Property Status() As Integer Implements IEvent.Status
			Get
				Return Me._Status
			End Get

			Set(ByVal value As Integer)
				SetPropertyValue("Status", Me._Status, value)
			End Set
		End Property
		<Size(250)> _
		Public Property Subject() As String Implements IEvent.Subject
			Get
				Return Me._Subject
			End Get

			Set(ByVal value As String)
				SetPropertyValue("Subject", Me._Subject, value)
			End Set
		End Property
		<Browsable(False)> _
		Public Property Type() As Integer Implements IEvent.Type
			Get
				Return Me._Type
			End Get

			Set(ByVal value As Integer)
				SetPropertyValue("Type", Me._Type, value)
			End Set
		End Property
		<Association("Activity-Employees", UseAssociationNameAsIntermediateTableName := True)> _
		Public ReadOnly Property Employees() As XPCollection(Of Employee)
			Get
				Return GetCollection(Of Employee)("Employees")
			End Get
		End Property
		Public Overrides Sub AfterConstruction()
			MyBase.AfterConstruction()
			Me.StartOn = DateTime.Now
			Me.EndOn = Me.StartOn.AddHours(1)
			Me.Employees.Add(Session.GetObjectByKey(Of Employee)(SecuritySystem.CurrentUserId))
		End Sub
		Public Sub UpdateEmployeeIds()
			Me._EmployeeIds = String.Empty
			Me.Employees.SuspendChangedEvents()
			Try
				For Each activityUser As Employee In Me.Employees
					Me._EmployeeIds &= String.Format("<ResourceId Type=""{0}"" Value=""{1}"" />", activityUser.Id.GetType().FullName, activityUser.Id)
				Next activityUser
			Finally
				Me.Employees.ResumeChangedEvents()
			End Try
			Me._EmployeeIds = "<ResourceIds>" & Me._EmployeeIds & "</ResourceIds>"
		End Sub
		Protected Overrides Overloads Function CreateCollection(Of T)(ByVal [property] As XPMemberInfo) As XPCollection(Of T)
			Dim result As XPCollection(Of T) = MyBase.CreateCollection(Of T)([property])
			If [property].Name = "Employees" Then
				AddHandler result.ListChanged, AddressOf Employees_ListChanged
			End If
			Return result
		End Function
		Private Sub UpdateEmployees()
			Me.Employees.SuspendChangedEvents()
			Try
				Do While Me.Employees.Count > 0
					Me.Employees.Remove(Me.Employees(0))
				Loop
				If (Not String.IsNullOrEmpty(_EmployeeIds)) Then
					Dim xmlDocument As New XmlDocument()
					xmlDocument.LoadXml(_EmployeeIds)
					For Each xmlNode As XmlNode In xmlDocument.DocumentElement.ChildNodes
						Dim activityUser As Employee = Session.GetObjectByKey(Of Employee)(New Guid(xmlNode.Attributes("Value").Value))
						If activityUser IsNot Nothing Then
							Me.Employees.Add(activityUser)
						End If
					Next xmlNode
				End If
			Finally
				Me.Employees.ResumeChangedEvents()
			End Try
		End Sub
		Private Sub Employees_ListChanged(ByVal sender As Object, ByVal e As ListChangedEventArgs)
			If e.ListChangedType = ListChangedType.ItemAdded OrElse e.ListChangedType = ListChangedType.ItemDeleted Then
				UpdateEmployeeIds()
				OnChanged("ResourceId")
			End If
		End Sub
		Private recurrenceInfoXml_Renamed As String
		<DevExpress.Xpo.DisplayName("Recurrence"), Size(SizeAttribute.Unlimited)> _
		Public Property RecurrenceInfoXml() As String Implements ISupportRecurrences.RecurrenceInfoXml
			Get
				Return recurrenceInfoXml_Renamed
			End Get
			Set(ByVal value As String)
				If recurrenceInfoXml_Renamed <> value Then
					recurrenceInfoXml_Renamed = value
					OnChanged("RecurrenceInfoXml")
				End If
			End Set
		End Property
		<Persistent("RecurrencePattern")> _
		Private recurrencePattern_Renamed As Activity
		Public Property RecurrencePattern() As IEvent Implements ISupportRecurrences.RecurrencePattern
			Get
				Return recurrencePattern_Renamed
			End Get
			Set(ByVal value As IEvent)
				recurrencePattern_Renamed = CType(value, Activity)
				OnChanged("RecurrencePattern")
			End Set
		End Property
		Protected Overrides Sub OnLoaded()
			MyBase.OnLoaded()
			If Employees.IsLoaded Then
				Employees.Reload()
			End If
		End Sub
	End Class
End Namespace