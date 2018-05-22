Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Xpo
Imports System.Drawing
Imports System.Security
Imports System.ComponentModel
Imports System.Collections.Generic
Imports DevExpress.Persistent.Base
Imports DevExpress.ExpressApp.Filtering
Imports DevExpress.Persistent.Validation
Imports DevExpress.Persistent.Base.General
Imports DevExpress.Persistent.Base.Security

Namespace WinWebSolution.Module
	<NavigationItem, ImageName("BO_User"), DefaultProperty("UserName")> _
	Public Class Employee
		Inherits BasePersistentObject
		Implements IPerson, IUser, IUserWithRoles, IAuthenticationActiveDirectoryUser, IAuthenticationStandardUser, IResource
		Private _FirstName As String
		Private _Email As String
		Private _LastName As String
		Private _MiddleName As String
		Private _Birthday As DateTime
		Private _IsActive As Boolean = True
		Private _ChangePasswordOnFirstLogon As Boolean
		Private _Permissions As List(Of IPermission)
		<Persistent("Color")> _
		Private _Color As Integer
		Private _Caption As String
		Private _UserName As String
		Private _StoredPassword As String
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
			_Permissions = New List(Of IPermission)()
		End Sub
		Public Overrides Sub AfterConstruction()
			MyBase.AfterConstruction()
			_Color = Color.White.ToArgb()
		End Sub
		<NonPersistent> _
		Public Property Color() As Color
			Get
				Return Color.FromArgb(_Color)
			End Get
			Set(ByVal value As Color)
				SetPropertyValue("Color", _Color, value.ToArgb())
			End Set
		End Property
		<Association("Employee-Group", UseAssociationNameAsIntermediateTableName := True), RuleRequiredField(Nothing, DefaultContexts.Save, TargetCriteria := "IsActive", CustomMessageTemplate := "You cannot save an Active employee without Groups")> _
		Public ReadOnly Property Groups() As XPCollection(Of Group)
			Get
				Return GetCollection(Of Group)("Groups")
			End Get
		End Property
		<Association("Activity-Employees", UseAssociationNameAsIntermediateTableName := True)> _
		Public ReadOnly Property Activities() As XPCollection(Of Activity)
			Get
				Return GetCollection(Of Activity)("Activities")
			End Get
		End Property
		<MemberDesignTimeVisibility(False)> _
		Public Property StoredPassword() As String
			Get
				Return _StoredPassword
			End Get
			Set(ByVal value As String)
				SetPropertyValue("StoredPassword", _StoredPassword, value)
			End Set
		End Property
		#Region "IUserWithRoles Members"
		Private ReadOnly Property IUserWithRoles_Roles() As IList(Of IRole) Implements IUserWithRoles.Roles
			Get
				Return New ListConverter(Of IRole, Group)(Groups)
			End Get
		End Property
		#End Region
		#Region "IUser Members"
		Public Property IsActive() As Boolean Implements IUser.IsActive
			Get
				Return _IsActive
			End Get
			Set(ByVal value As Boolean)
				SetPropertyValue("IsActive", _IsActive, value)
			End Set
		End Property
        Public ReadOnly Property Permissions() As IList(Of IPermission) Implements IUser.Permissions
            Get
                _Permissions.Clear()
                For Each role As IRole In Groups
                    _Permissions.AddRange(role.Permissions)
                Next role
                Return _Permissions.AsReadOnly()
            End Get
        End Property
		Public Sub ReloadPermissions() Implements IUser.ReloadPermissions
			Groups.Reload()
			For Each group As Group In Groups
				group.PersistentPermissions.Reload()
			Next group
		End Sub
		Private ReadOnly Property IUser_UserName() As String Implements IUser.UserName
			Get
				Return UserName
			End Get
		End Property
		#End Region
#Region "IAuthenticationActiveDirectoryUser Members"
        <RuleRequiredField(Nothing, "Save", "The user name must not be empty"), RuleUniqueValue(Nothing, "Save", "The login with the entered UserName was already registered within the system")> _
        Public Property UserName() As String Implements IAuthenticationActiveDirectoryUser.UserName
            Get
                Return _UserName
            End Get
            Set(ByVal value As String)
                SetPropertyValue("UserName", _UserName, value)
            End Set
        End Property
#End Region
#Region "IAuthenticationStandardUser Members"
        Private ReadOnly Property IAuthenticationStandardUser_UserName() As String Implements IAuthenticationStandardUser.UserName
            Get
                Return UserName
            End Get
        End Property
        Public Property ChangePasswordOnFirstLogon() As Boolean Implements IAuthenticationStandardUser.ChangePasswordOnFirstLogon
            Get
                Return _ChangePasswordOnFirstLogon
            End Get
            Set(ByVal value As Boolean)
                SetPropertyValue("ChangePasswordOnFirstLogon", _ChangePasswordOnFirstLogon, value)
            End Set
        End Property
        Public Function ComparePassword(ByVal password As String) As Boolean Implements IAuthenticationStandardUser.ComparePassword
            Return New PasswordCryptographer().AreEqual(StoredPassword, password)
        End Function
        Public Sub SetPassword(ByVal password As String) Implements IAuthenticationStandardUser.SetPassword
            StoredPassword = New PasswordCryptographer().GenerateSaltedPassword(password)
        End Sub
#End Region
		#Region "IResource Members"
		Public Property Caption() As String Implements IResource.Caption
			Get
				Return _Caption
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Caption", _Caption, value)
			End Set
		End Property
		<Browsable(False)> _
		Public ReadOnly Property Id() As Object Implements IResource.Id
			Get
				Return Oid
			End Get
		End Property
		<Browsable(False)> _
		Public ReadOnly Property OleColor() As Integer Implements IResource.OleColor
			Get
				Return ColorTranslator.ToOle(Color.FromArgb(_Color))
			End Get
		End Property
		#End Region
#Region "IPerson Members"
        Public Property FirstName() As String Implements IPerson.FirstName
            Get
                Return _FirstName
            End Get
            Set(ByVal value As String)
                SetPropertyValue("FirstName", _FirstName, value)
            End Set
        End Property
        Public Property LastName() As String Implements IPerson.LastName
            Get
                Return _LastName
            End Get
            Set(ByVal value As String)
                SetPropertyValue("LastName", _LastName, value)
            End Set
        End Property
        Public Property MiddleName() As String Implements IPerson.MiddleName
            Get
                Return _MiddleName
            End Get
            Set(ByVal value As String)
                SetPropertyValue("MiddleName", _MiddleName, value)
            End Set
        End Property
        Public Property Birthday() As DateTime Implements IPerson.Birthday
            Get
                Return _Birthday
            End Get
            Set(ByVal value As DateTime)
                SetPropertyValue("Birthday", _Birthday, value)
            End Set
        End Property
        <SizeAttribute(255)> _
        Public Property Email() As String Implements IPerson.Email
            Get
                Return _Email
            End Get
            Set(ByVal value As String)
                SetPropertyValue("Email", _Email, value)
            End Set
        End Property
        <PersistentAlias("concat(FirstName, MiddleName, LastName)"), SearchMemberOptions(SearchMemberMode.Include)> _
        Public ReadOnly Property FullName() As String Implements IPerson.FullName
            Get
                Return Convert.ToString(EvaluateAlias("FullName"))
            End Get
        End Property
#End Region
	End Class
End Namespace