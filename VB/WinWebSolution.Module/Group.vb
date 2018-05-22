Imports Microsoft.VisualBasic
Imports DevExpress.Xpo
Imports System.Security
Imports System.ComponentModel
Imports DevExpress.Persistent.Base
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports DevExpress.Persistent.Validation
Imports DevExpress.Persistent.Base.Security

Namespace WinWebSolution.Module
	<ImageName("BO_Role"), DefaultProperty("Name"), RuleCriteria(Nothing, "Delete", "Employees.Count == 0", "Cannot delete the role because there are users that reference it", SkipNullOrEmptyValues := True)> _
	Public Class Group
		Inherits BasePersistentObject
		Implements IRole, ICustomizableRole
		Public Const DefaultAdministratorsGroupName As String = "Administrators"
		Public Const DefaultUsersGroupName As String = "Users"
		Private _Name As String
		Private _Permissions As New List(Of IPermission)()
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Public Function GetPermissions(ByVal persistentPermissions As IList(Of IPersistentPermission)) As ReadOnlyCollection(Of IPermission)
			_Permissions.Clear()
			For Each persistentPermission As IPersistentPermission In persistentPermissions
				If persistentPermission.Permission IsNot Nothing Then
					_Permissions.Add(persistentPermission.Permission)
				End If
			Next persistentPermission
			Return _Permissions.AsReadOnly()
		End Function
		<Association("Employee-Group", UseAssociationNameAsIntermediateTableName := True)> _
		Public ReadOnly Property Employees() As XPCollection(Of Employee)
			Get
				Return GetCollection(Of Employee)("Employees")
			End Get
		End Property
		<Aggregated, Association("Group-PersistentPermissionObjects"), DevExpress.Xpo.DisplayName("Permissions")> _
		Public ReadOnly Property PersistentPermissions() As XPCollection(Of PersistentPermissionObject)
			Get
				Return GetCollection(Of PersistentPermissionObject)("PersistentPermissions")
			End Get
		End Property
		Public Function AddPermission(ByVal permission As IPermission) As PersistentPermissionObject
			Dim result As New PersistentPermissionObject(Session, permission)
			PersistentPermissions.Add(result)
			Return result
		End Function
		Private ReadOnly Property Users() As IList(Of IUser) Implements IRole.Users
			Get
				Return New ListConverter(Of IUser, Employee)(Employees)
			End Get
		End Property
		#Region "IRole Members"
		<RuleRequiredField(Nothing, "Save", "The group name must not be empty"), RuleUniqueValue(Nothing, "Save", "The group with the entered Name was already registered within the system")> _
		Public Property Name() As String Implements IRole.Name
			Get
				Return _Name
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Name", _Name, value)
			End Set
		End Property
		Private ReadOnly Property Permissions() As ReadOnlyCollection(Of IPermission) Implements IRole.Permissions
			Get
				Return GetPermissions(New ListConverter(Of IPersistentPermission, PersistentPermissionObject)(PersistentPermissions))
			End Get
		End Property
		#End Region
		#Region "ICustomizableRole Members"
		Private Sub ICustomizableRole_AddPermission(ByVal permission As IPermission) Implements ICustomizableRole.AddPermission
			AddPermission(permission)
		End Sub
		#End Region
	End Class
End Namespace