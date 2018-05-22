Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp.Win

Namespace WinWebSolution.Win
	Partial Public Class WinWebSolutionWindowsFormsApplication
		Inherits WinApplication
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub WinWebSolutionWindowsFormsApplication_DatabaseVersionMismatch(ByVal sender As Object, ByVal e As DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs) Handles MyBase.DatabaseVersionMismatch
			e.Updater.Update()
			e.Handled = True
		End Sub
	End Class
End Namespace
