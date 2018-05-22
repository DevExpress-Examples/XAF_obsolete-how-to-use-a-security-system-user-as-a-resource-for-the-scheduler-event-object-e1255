Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Security
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.Base.General
Imports DevExpress.Persistent.Base.Security
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Validation
Imports DevExpress.Xpo

Namespace WinWebSolution.Module
	<NavigationItem, DefaultProperty("UserName")> _
	Public Class Employee
		Inherits Person
		Implements IUserWithRoles, IAuthenticationActiveDirectoryUser, IAuthenticationStandardUser, IResource
		Private user As UserImpl
		Private permissions_Renamed As List(Of IPermission)
		<Persistent("Color")> _
		Private _Color As Integer
		Private _Caption As String
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
			Me.permissions_Renamed = New List(Of IPermission)()
			Me.user = New UserImpl(Me)
		End Sub
		<Association("User-Role", UseAssociationNameAsIntermediateTableName := True), RuleRequiredField("RuleRequiredField for IUserWithRoles.Roles", DefaultContexts.Save)> _
		Public ReadOnly Property Groups() As XPCollection(Of Group)
			Get
				Return GetCollection(Of Group)("Groups")
			End Get
		End Property
        Public Property UserName() As String Implements IAuthenticationActiveDirectoryUser.UserName
            Get
                Return Me.user.UserName
            End Get

            Set(ByVal value As String)
                Me.user.UserName = value
                OnChanged("UserName")
            End Set
        End Property
        Private ReadOnly Property IAuthenticationStandardUser_UserName() As String Implements IAuthenticationStandardUser.UserName
            Get
                Return Me.user.UserName
            End Get
        End Property
        Private ReadOnly Property IUser_UserName() As String Implements DevExpress.Persistent.Base.Security.IUser.UserName
            Get
                Return Me.user.UserName
            End Get
        End Property
		Public Property ChangePasswordOnFirstLogon() As Boolean Implements IAuthenticationStandardUser.ChangePasswordOnFirstLogon
			Get
				Return Me.user.ChangePasswordAfterLogon
			End Get

			Set(ByVal value As Boolean)
				Me.user.ChangePasswordAfterLogon = value
				OnChanged("ChangePasswordOnFirstLogon")
			End Set
		End Property
		Public Property IsActive() As Boolean Implements DevExpress.Persistent.Base.Security.IUser.IsActive
			Get
				Return Me.user.IsActive
			End Get

			Set(ByVal value As Boolean)
				Me.user.IsActive = value
				OnChanged("IsActive")
			End Set
		End Property
        Public ReadOnly Property Permissions() As IList(Of IPermission) Implements IUser.Permissions
            Get
                Me.permissions_Renamed.Clear()
                For Each role As Group In Me.Groups
                    Me.permissions_Renamed.AddRange(role.Permissions)
                Next role

                Return Me.permissions_Renamed.AsReadOnly()
            End Get
        End Property
		Private ReadOnly Property IUserWithRoles_Roles() As IList(Of IRole) Implements IUserWithRoles.Roles
			Get
				Return New ListConverter(Of IRole, Group)(Me.Groups)
			End Get
		End Property
		Public Property Caption() As String Implements IResource.Caption
			Get
				Return Me._Caption
			End Get

			Set(ByVal value As String)
				SetPropertyValue("Caption", Me._Caption, value)
			End Set
		End Property
		<NonPersistent, Browsable(False)> _
		Public ReadOnly Property Id() As Object Implements IResource.Id
			Get
				Return Me.Oid
			End Get
		End Property
		<NonPersistent, Browsable(False)> _
		Public ReadOnly Property OleColor() As Integer Implements IResource.OleColor
			Get
				Return ColorTranslator.ToOle(Color.FromArgb(Me._Color))
			End Get
		End Property
		<NonPersistent> _
		Public Property Color() As Color
			Get
				Return Color.FromArgb(Me._Color)
			End Get

			Set(ByVal value As Color)
				SetPropertyValue("Color", Me._Color, value.ToArgb())
			End Set
		End Property
		<Association("Activity-Employees", GetType(Activity), UseAssociationNameAsIntermediateTableName := True)> _
		Public ReadOnly Property Activities() As XPCollection
			Get
				Return GetCollection("Activities")
			End Get
		End Property
		<Persistent> _
		Private Property StoredPassword() As String
			Get
				Return Me.user.StoredPassword
			End Get

			Set(ByVal value As String)
				Me.user.StoredPassword = value
				OnChanged("StoredPassword")
			End Set
		End Property
		Public Sub SetPassword(ByVal password As String) Implements IAuthenticationStandardUser.SetPassword
			Me.user.SetPassword(password)
		End Sub
		Public Sub ReloadPermissions() Implements DevExpress.Persistent.Base.Security.IUser.ReloadPermissions
			Me.Groups.Reload()
			For Each role As Group In Me.Groups
				role.PersistentPermissions.Reload()
			Next role
		End Sub
		Public Function ComparePassword(ByVal password As String) As Boolean Implements IAuthenticationStandardUser.ComparePassword
			Return Me.user.ComparePassword(password)
		End Function
		Public Overrides Sub AfterConstruction()
			MyBase.AfterConstruction()
			Me._Color = Color.White.ToArgb()
		End Sub
	End Class
End Namespace