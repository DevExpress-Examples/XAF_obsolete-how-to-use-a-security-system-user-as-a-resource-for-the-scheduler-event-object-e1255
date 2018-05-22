Imports Microsoft.VisualBasic
Imports System
Namespace WinWebSolution.Win
	Partial Public Class WinWebSolutionWindowsFormsApplication
		''' <summary> 
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary> 
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Component Designer generated code"

		''' <summary> 
		''' Required method for Designer support - do not modify 
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.module1 = New DevExpress.ExpressApp.SystemModule.SystemModule()
			Me.module2 = New DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule()
			Me.module3 = New WinWebSolution.Module.WinWebSolutionModule()
			Me.module4 = New WinWebSolution.Module.Win.WinWebSolutionWindowsFormsModule()
			Me.module5 = New DevExpress.ExpressApp.Validation.ValidationModule()
			Me.module6 = New DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule()
			Me.module7 = New DevExpress.ExpressApp.Validation.Win.ValidationWindowsFormsModule()
			Me.securityModule1 = New DevExpress.ExpressApp.Security.SecurityModule()
			Me.schedulerWindowsFormsModule1 = New DevExpress.ExpressApp.Scheduler.Win.SchedulerWindowsFormsModule()
			Me.schedulerModuleBase1 = New DevExpress.ExpressApp.Scheduler.SchedulerModuleBase()
			Me.securityComplex1 = New DevExpress.ExpressApp.Security.SecurityComplex()
			Me.securityComplex2 = New DevExpress.ExpressApp.Security.SecurityComplex()
			Me.securityComplex3 = New DevExpress.ExpressApp.Security.SecurityComplex()
			Me.authenticationStandard1 = New DevExpress.ExpressApp.Security.AuthenticationStandard()
			Me.authenticationStandard2 = New DevExpress.ExpressApp.Security.AuthenticationStandard()
			Me.authenticationStandard3 = New DevExpress.ExpressApp.Security.AuthenticationStandard()
			CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
			' 
			' module5
			' 
			Me.module5.AllowValidationDetailsAccess = True
			' 
			' securityComplex1
			' 
			Me.securityComplex1.Authentication = Nothing
			Me.securityComplex1.IsGrantedForNonExistentPermission = False
			Me.securityComplex1.RoleType = Nothing
			Me.securityComplex1.UserType = Nothing
			' 
			' securityComplex2
			' 
			Me.securityComplex2.Authentication = Nothing
			Me.securityComplex2.IsGrantedForNonExistentPermission = False
			Me.securityComplex2.RoleType = Nothing
			Me.securityComplex2.UserType = Nothing
			' 
			' securityComplex3
			' 
			Me.securityComplex3.Authentication = Me.authenticationStandard3
			Me.securityComplex3.IsGrantedForNonExistentPermission = False
			Me.securityComplex3.RoleType = GetType(WinWebSolution.Module.Group)
			Me.securityComplex3.UserType = GetType(WinWebSolution.Module.Employee)
			' 
			' authenticationStandard1
			' 
			Me.authenticationStandard1.LogonParametersType = GetType(DevExpress.ExpressApp.Security.AuthenticationStandardLogonParameters)
			' 
			' authenticationStandard2
			' 
			Me.authenticationStandard2.LogonParametersType = GetType(DevExpress.ExpressApp.Security.AuthenticationStandardLogonParameters)
			' 
			' authenticationStandard3
			' 
			Me.authenticationStandard3.LogonParametersType = GetType(DevExpress.ExpressApp.Security.AuthenticationStandardLogonParameters)
			' 
			' WinWebSolutionWindowsFormsApplication
			' 
			Me.ApplicationName = "WinWebSolution"
			Me.Modules.Add(Me.module1)
			Me.Modules.Add(Me.module2)
			Me.Modules.Add(Me.module6)
			Me.Modules.Add(Me.module3)
			Me.Modules.Add(Me.module4)
			Me.Modules.Add(Me.module5)
			Me.Modules.Add(Me.module7)
			Me.Modules.Add(Me.securityModule1)
			Me.Modules.Add(Me.schedulerModuleBase1)
			Me.Modules.Add(Me.schedulerWindowsFormsModule1)
			Me.Security = Me.securityComplex3
'			Me.DatabaseVersionMismatch += New System.EventHandler(Of DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs)(Me.WinWebSolutionWindowsFormsApplication_DatabaseVersionMismatch);
			CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

		End Sub

		#End Region

		Private module1 As DevExpress.ExpressApp.SystemModule.SystemModule
		Private module2 As DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule
		Private module3 As WinWebSolution.Module.WinWebSolutionModule
		Private module4 As WinWebSolution.Module.Win.WinWebSolutionWindowsFormsModule
		Private module5 As DevExpress.ExpressApp.Validation.ValidationModule
		Private module6 As DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule
		Private module7 As DevExpress.ExpressApp.Validation.Win.ValidationWindowsFormsModule
		Private securityModule1 As DevExpress.ExpressApp.Security.SecurityModule
		Private schedulerWindowsFormsModule1 As DevExpress.ExpressApp.Scheduler.Win.SchedulerWindowsFormsModule
		Private schedulerModuleBase1 As DevExpress.ExpressApp.Scheduler.SchedulerModuleBase
		Private securityComplex1 As DevExpress.ExpressApp.Security.SecurityComplex
		Private securityComplex2 As DevExpress.ExpressApp.Security.SecurityComplex
		Private securityComplex3 As DevExpress.ExpressApp.Security.SecurityComplex
		Private authenticationStandard1 As DevExpress.ExpressApp.Security.AuthenticationStandard
		Private authenticationStandard2 As DevExpress.ExpressApp.Security.AuthenticationStandard
		Private authenticationStandard3 As DevExpress.ExpressApp.Security.AuthenticationStandard
	End Class
End Namespace
