Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp.Web

Namespace WinWebSolution.Web
	Partial Public Class WinWebSolutionAspNetApplication
		Inherits WebApplication
		Private module1 As DevExpress.ExpressApp.SystemModule.SystemModule
		Private module2 As DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule
		Private module3 As WinWebSolution.Module.WinWebSolutionModule
		Private module4 As WinWebSolution.Module.Web.WinWebSolutionAspNetModule
		Private securityModule1 As DevExpress.ExpressApp.Security.SecurityModule
		Private module6 As DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule
		Private schedulerAspNetModule1 As DevExpress.ExpressApp.Scheduler.Web.SchedulerAspNetModule
		Private schedulerModuleBase1 As DevExpress.ExpressApp.Scheduler.SchedulerModuleBase
		Private securityComplex1 As DevExpress.ExpressApp.Security.SecurityComplex
		Private systemModule1 As DevExpress.ExpressApp.SystemModule.SystemModule
		Private systemAspNetModule1 As DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule
		Private authenticationStandard1 As DevExpress.ExpressApp.Security.AuthenticationStandard
		Private module5 As DevExpress.ExpressApp.Validation.ValidationModule

		Public Sub New()
			InitializeComponent()
		End Sub
		Protected Overrides Sub OnSetupComplete()
			MyBase.OnSetupComplete()
			Dim str As ShowViewStrategy = TryCast(Me.ShowViewStrategy, ShowViewStrategy)
			If str Is Nothing Then
				str = New ShowViewStrategy(Me)
				Me.ShowViewStrategy = str
			End If
			str.CollectionsEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit

		End Sub
		Private Sub WinWebSolutionAspNetApplication_DatabaseVersionMismatch(ByVal sender As Object, ByVal e As DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs) Handles MyBase.DatabaseVersionMismatch
			e.Updater.Update()
			e.Handled = True
		End Sub
		Private Sub InitializeComponent()
			Me.module1 = New DevExpress.ExpressApp.SystemModule.SystemModule()
			Me.module2 = New DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule()
			Me.module3 = New WinWebSolution.Module.WinWebSolutionModule()
			Me.module4 = New WinWebSolution.Module.Web.WinWebSolutionAspNetModule()
			Me.module5 = New DevExpress.ExpressApp.Validation.ValidationModule()
			Me.module6 = New DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule()
			Me.securityModule1 = New DevExpress.ExpressApp.Security.SecurityModule()
			Me.schedulerAspNetModule1 = New DevExpress.ExpressApp.Scheduler.Web.SchedulerAspNetModule()
			Me.schedulerModuleBase1 = New DevExpress.ExpressApp.Scheduler.SchedulerModuleBase()
			Me.securityComplex1 = New DevExpress.ExpressApp.Security.SecurityComplex()
			Me.systemModule1 = New DevExpress.ExpressApp.SystemModule.SystemModule()
			Me.systemAspNetModule1 = New DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule()
			Me.authenticationStandard1 = New DevExpress.ExpressApp.Security.AuthenticationStandard()
			CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
			' 
			' module1
			' 
			Me.module1.AdditionalBusinessClasses.Add(GetType(DevExpress.Xpo.XPObjectType))
			' 
			' module5
			' 
			Me.module5.AllowValidationDetailsAccess = True
			' 
			' schedulerAspNetModule1
			' 
			Me.schedulerAspNetModule1.Description = "Uses the ASPxScheduler controls suite to display DevExpress.Persistent.Base.IEven" & "t objects in Web XAF applications."
			Me.schedulerAspNetModule1.RequiredModuleTypes.Add(GetType(DevExpress.ExpressApp.SystemModule.SystemModule))
			Me.schedulerAspNetModule1.RequiredModuleTypes.Add(GetType(DevExpress.ExpressApp.Scheduler.SchedulerModuleBase))
			' 
			' securityComplex1
			' 
			Me.securityComplex1.Authentication = Me.authenticationStandard1
			Me.securityComplex1.IsGrantedForNonExistentPermission = False
			Me.securityComplex1.RoleType = GetType(WinWebSolution.Module.Group)
			Me.securityComplex1.UserType = GetType(WinWebSolution.Module.Employee)
			' 
			' systemModule1
			' 
			Me.systemModule1.AdditionalBusinessClasses.Add(GetType(DevExpress.Xpo.XPObjectType))
			' 
			' authenticationStandard1
			' 
			Me.authenticationStandard1.LogonParametersType = GetType(DevExpress.ExpressApp.Security.AuthenticationStandardLogonParameters)
			' 
			' WinWebSolutionAspNetApplication
			' 
			Me.ApplicationName = "WinWebSolution"
			Me.Modules.Add(Me.systemModule1)
			Me.Modules.Add(Me.systemAspNetModule1)
			Me.Modules.Add(Me.schedulerModuleBase1)
			Me.Modules.Add(Me.schedulerAspNetModule1)
			Me.Modules.Add(Me.securityModule1)
			Me.Modules.Add(Me.module6)
			Me.Modules.Add(Me.module5)
			Me.Modules.Add(Me.module3)
			Me.Modules.Add(Me.module4)
			Me.Security = Me.securityComplex1
'			Me.DatabaseVersionMismatch += New System.EventHandler(Of DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs)(Me.WinWebSolutionAspNetApplication_DatabaseVersionMismatch);
			CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

		End Sub
	End Class
End Namespace
