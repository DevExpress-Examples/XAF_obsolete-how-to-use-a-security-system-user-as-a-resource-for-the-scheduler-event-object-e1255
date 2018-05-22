Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.ComponentModel
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.Base.Security
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Xpo

Namespace WinWebSolution.Module
	<DefaultProperty("Name")> _
	Public Class Group
		Inherits RoleBase
		Implements IRole
                                
                                Public Const DefaultAdministratorsGroupName As String = "Administrators"
		Public Const DefaultUsersGroupName As String = "Users"
		
                                Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		<Association("User-Role", UseAssociationNameAsIntermediateTableName := True)> _
		Public ReadOnly Property Employees() As XPCollection(Of Employee)
			Get
				Return GetCollection(Of Employee)("Employees")
			End Get
        End Property
        Private Property IRole_UserName() As String Implements IRole.Name
            Get
                Return Me.Name
            End Get
            Set(ByVal value As String)
                Me.Name = value
                OnChanged("Name")
            End Set
        End Property
        Private ReadOnly Property IRole_Permissions() As System.Collections.ObjectModel.ReadOnlyCollection(Of System.Security.IPermission) Implements IRole.Permissions
            Get
                Return Me.Permissions
            End Get
        End Property
		Private ReadOnly Property Users() As IList(Of IUser) Implements IRole.Users
			Get
				Return New ListConverter(Of IUser, Employee)(Me.Employees)
			End Get
		End Property
	End Class
End Namespace
