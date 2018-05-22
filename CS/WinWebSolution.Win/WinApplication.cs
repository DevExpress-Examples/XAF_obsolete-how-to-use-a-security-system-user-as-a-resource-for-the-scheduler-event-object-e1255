using System;
using DevExpress.ExpressApp.Win;

namespace WinWebSolution.Win {
    public partial class WinWebSolutionWindowsFormsApplication : WinApplication {
        public WinWebSolutionWindowsFormsApplication() {
            InitializeComponent();
        }

        private void WinWebSolutionWindowsFormsApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e) {
            e.Updater.Update();
            e.Handled = true;
        }
    }
}
