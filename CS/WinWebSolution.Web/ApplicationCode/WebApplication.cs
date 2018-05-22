using System;
using DevExpress.ExpressApp.Web;

namespace WinWebSolution.Web {
    public partial class WinWebSolutionAspNetApplication : WebApplication {
        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule module2;
        private WinWebSolution.Module.WinWebSolutionModule module3;
        private WinWebSolution.Module.Web.WinWebSolutionAspNetModule module4;
        private DevExpress.ExpressApp.Security.SecurityModule securityModule1;
        private DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule module6;
        private DevExpress.ExpressApp.Scheduler.Web.SchedulerAspNetModule schedulerAspNetModule1;
        private DevExpress.ExpressApp.Scheduler.SchedulerModuleBase schedulerModuleBase1;
        private DevExpress.ExpressApp.Security.SecurityComplex securityComplex1;
        private DevExpress.ExpressApp.SystemModule.SystemModule systemModule1;
        private DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule systemAspNetModule1;
        private DevExpress.ExpressApp.Security.AuthenticationStandard authenticationStandard1;
        private DevExpress.ExpressApp.Validation.ValidationModule module5;

        public WinWebSolutionAspNetApplication() {
            InitializeComponent();
        }
        protected override void OnSetupComplete() {
            base.OnSetupComplete();
            ((ShowViewStrategy)base.ShowViewStrategy).CollectionsEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
        }
        private void WinWebSolutionAspNetApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e) {
            e.Updater.Update();
            e.Handled = true;
        }
        private void InitializeComponent() {
            this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.module2 = new DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule();
            this.module3 = new WinWebSolution.Module.WinWebSolutionModule();
            this.module4 = new WinWebSolution.Module.Web.WinWebSolutionAspNetModule();
            this.module5 = new DevExpress.ExpressApp.Validation.ValidationModule();
            this.module6 = new DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule();
            this.securityModule1 = new DevExpress.ExpressApp.Security.SecurityModule();
            this.schedulerAspNetModule1 = new DevExpress.ExpressApp.Scheduler.Web.SchedulerAspNetModule();
            this.schedulerModuleBase1 = new DevExpress.ExpressApp.Scheduler.SchedulerModuleBase();
            this.securityComplex1 = new DevExpress.ExpressApp.Security.SecurityComplex();
            this.systemModule1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.systemAspNetModule1 = new DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule();
            this.authenticationStandard1 = new DevExpress.ExpressApp.Security.AuthenticationStandard();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // module1
            // 
            this.module1.AdditionalBusinessClasses.Add(typeof(DevExpress.Xpo.XPObjectType));
            // 
            // module5
            // 
            this.module5.AllowValidationDetailsAccess = true;
            // 
            // schedulerAspNetModule1
            // 
            this.schedulerAspNetModule1.Description = "Uses the ASPxScheduler controls suite to display DevExpress.Persistent.Base.IEven" +
                "t objects in Web XAF applications.";
            this.schedulerAspNetModule1.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.SystemModule.SystemModule));
            this.schedulerAspNetModule1.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Scheduler.SchedulerModuleBase));
            // 
            // securityComplex1
            // 
            this.securityComplex1.Authentication = this.authenticationStandard1;
            this.securityComplex1.IsGrantedForNonExistentPermission = false;
            this.securityComplex1.RoleType = typeof(WinWebSolution.Module.Group);
            this.securityComplex1.UserType = typeof(WinWebSolution.Module.Employee);
            // 
            // systemModule1
            // 
            this.systemModule1.AdditionalBusinessClasses.Add(typeof(DevExpress.Xpo.XPObjectType));
            // 
            // authenticationStandard1
            // 
            this.authenticationStandard1.LogonParametersType = typeof(DevExpress.ExpressApp.Security.AuthenticationStandardLogonParameters);
            // 
            // WinWebSolutionAspNetApplication
            // 
            this.ApplicationName = "WinWebSolution";
            this.Modules.Add(this.systemModule1);
            this.Modules.Add(this.systemAspNetModule1);
            this.Modules.Add(this.schedulerModuleBase1);
            this.Modules.Add(this.schedulerAspNetModule1);
            this.Modules.Add(this.securityModule1);
            this.Modules.Add(this.module6);
            this.Modules.Add(this.module5);
            this.Modules.Add(this.module3);
            this.Modules.Add(this.module4);
            this.Security = this.securityComplex1;
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.WinWebSolutionAspNetApplication_DatabaseVersionMismatch);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
    }
}
