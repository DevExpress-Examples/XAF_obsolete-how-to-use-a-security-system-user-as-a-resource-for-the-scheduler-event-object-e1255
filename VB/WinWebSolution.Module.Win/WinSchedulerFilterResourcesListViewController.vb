Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.Data.Filtering
Imports DevExpress.Xpo

Namespace WinWebSolution.Module
	Public Class WinSchedulerFilterResourcesListViewController
		Inherits SchedulerFilterResourcesListViewControllerBase
		Protected Overrides Sub FilterResourcesDataSource(ByVal resourcesDataSource As Object, ByVal criteria As CriteriaOperator)
			If TypeOf resourcesDataSource Is XPCollection Then
				CType(resourcesDataSource, XPCollection).Criteria = criteria
			End If
		End Sub
	End Class
End Namespace