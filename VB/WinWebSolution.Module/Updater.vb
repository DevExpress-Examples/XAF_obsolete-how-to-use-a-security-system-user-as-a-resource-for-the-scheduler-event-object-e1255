Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp.Updating
Imports DevExpress.ExpressApp.Security
Imports DevExpress.ExpressApp

Namespace WinWebSolution.Module
	Public Class Updater
		Inherits ModuleUpdater
		Public Sub New(ByVal objectSpace As ObjectSpace, ByVal currentDBVersion As Version)
			MyBase.New(objectSpace, currentDBVersion)
		End Sub
		Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
			MyBase.UpdateDatabaseAfterUpdateSchema()
			' If a user named 'Sam' doesn't exist in the database, create this user
			Dim user1 As Employee = ObjectSpace.FindObject(Of Employee)(New BinaryOperator("UserName", "Sam"))
			If user1 Is Nothing Then
				user1 = ObjectSpace.CreateObject(Of Employee)()
				user1.Color = Color.Red
				user1.UserName = "Sam"
				user1.FirstName = "Sam"
				user1.Caption = user1.UserName
				' Set a password if the standard authentication type is used
				user1.SetPassword(Nothing)
			End If
			' If a user named 'John' doesn't exist in the database, create this user
			Dim user2 As Employee = ObjectSpace.FindObject(Of Employee)(New BinaryOperator("UserName", "John"))
			If user2 Is Nothing Then
				user2 = ObjectSpace.CreateObject(Of Employee)()
				user2.Color = Color.Green
				user2.UserName = "John"
				user2.FirstName = "John"
				user2.Caption = user2.UserName
				' Set a password if the standard authentication type is used
				user2.SetPassword(Nothing)
			End If
			' If a role with the Administrators name doesn't exist in the database, create this role
			Dim adminRole As Group = ObjectSpace.FindObject(Of Group)(New BinaryOperator("Name", Group.DefaultAdministratorsGroupName))
			If adminRole Is Nothing Then
				adminRole = ObjectSpace.CreateObject(Of Group)()
				adminRole.Name = Group.DefaultAdministratorsGroupName
			End If
			' If a role with the Users name doesn't exist in the database, create this role
			Dim userRole As Group = ObjectSpace.FindObject(Of Group)(New BinaryOperator("Name", Group.DefaultUsersGroupName))
			If userRole Is Nothing Then
				userRole = ObjectSpace.CreateObject(Of Group)()
				userRole.Name = Group.DefaultUsersGroupName
			End If
			' Delete all permissions assigned to the Administrators and Users roles
			Do While adminRole.PersistentPermissions.Count > 0
				ObjectSpace.Delete(adminRole.PersistentPermissions(0))
			Loop
			Do While userRole.PersistentPermissions.Count > 0
				ObjectSpace.Delete(userRole.PersistentPermissions(0))
			Loop
			' Allow full access to all objects to the Administrators role
			adminRole.AddPermission(New ObjectAccessPermission(GetType(Object), ObjectAccess.AllAccess))
			' Allow editing the application model to the Administrators role
			adminRole.AddPermission(New EditModelPermission(ModelAccessModifier.Allow))
			' Save the Administrators role to the database
			adminRole.Save()
			' Allow full access to all objects to the Users role
			userRole.AddPermission(New ObjectAccessPermission(GetType(Object), ObjectAccess.AllAccess))
			' Deny full access to the User type objects to the Users role
			userRole.AddPermission(New ObjectAccessPermission(GetType(Employee), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny))
			userRole.AddPermission(New ObjectAccessPermission(GetType(Group), ObjectAccess.AllAccess, ObjectAccessModifier.Deny))
			' Deny editing the application model to the Users role
			userRole.AddPermission(New EditModelPermission(ModelAccessModifier.Deny))
			' Save the Users role to the database
			userRole.Save()
			' Add the Administrators role to the user1
			user1.Groups.Add(adminRole)
			' Add the Users role to the user2
			user2.Groups.Add(userRole)
			' Save the users to the database
			user1.Save()
			user2.Save()
		End Sub
	End Class
End Namespace
