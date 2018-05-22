Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp
Imports System.ComponentModel
Imports DevExpress.Xpo.Metadata

Namespace WinWebSolution.Module
	Public Interface IPerson
		Property Birthday() As DateTime
		Property Email() As String
		Property FirstName() As String
		ReadOnly Property FullName() As String
		Property LastName() As String
		Property MiddleName() As String
	End Interface
	<NonPersistent> _
	Public MustInherit Class BasePersistentObject
		Inherits XPCustomObject
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Private isDefaultPropertyAttributeInit As Boolean
		Private defaultPropertyMemberInfo As XPMemberInfo
#If MediumTrust Then
		Private _Oid As Guid = Guid.Empty
		<Browsable(False), Key(True), DevExpress.Persistent.Base.NonCloneable> _
		Public Property Oid() As Guid
			Get
				Return _Oid
			End Get
			Set(ByVal value As Guid)
				_Oid = value
			End Set
		End Property
#Else
		<Persistent("Oid"), Key(True), Browsable(False), MemberDesignTimeVisibility(False)> _
		Private _Oid As Guid = Guid.Empty
		<PersistentAlias("_Oid"), Browsable(False)> _
		Public ReadOnly Property Oid() As Guid
			Get
				Return _Oid
			End Get
		End Property
#End If
		Protected Overrides Sub OnSaving()
			MyBase.OnSaving()
			If Not(TypeOf Session Is NestedUnitOfWork) AndAlso Session.IsNewObject(Me) Then
				_Oid = XpoDefault.NewGuid()
			End If
		End Sub
		Public Overrides Function ToString() As String
		If (Not isDefaultPropertyAttributeInit) Then
			Dim attrib As DefaultPropertyAttribute = XafTypesInfo.Instance.FindTypeInfo(Me.GetType()).FindAttribute(Of DefaultPropertyAttribute)()
			If attrib IsNot Nothing Then
				defaultPropertyMemberInfo = ClassInfo.FindMember(attrib.Name)
			End If
			isDefaultPropertyAttributeInit = True
		End If
		If defaultPropertyMemberInfo IsNot Nothing Then
			Dim obj As Object = defaultPropertyMemberInfo.GetValue(Me)
			If obj IsNot Nothing Then
				Return obj.ToString()
			End If
		End If
		Return MyBase.ToString()
		End Function
	End Class
End Namespace