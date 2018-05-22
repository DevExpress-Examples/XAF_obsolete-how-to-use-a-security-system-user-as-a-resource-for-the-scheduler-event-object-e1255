Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.Data.Filtering
Imports DevExpress.Xpo

Namespace WinWebSolution.Module
	Public Class WebSchedulerFilterResourcesListViewController
		Inherits SchedulerFilterResourcesListViewControllerBase
		Protected Overrides Sub FilterResourcesDataSource(ByVal resourcesDataSource As Object, ByVal criteria As CriteriaOperator)
			If TypeOf resourcesDataSource Is XpoDataSource Then
				CType(resourcesDataSource, XpoDataSource).Criteria = criteria.ToString()
			End If
		End Sub
	End Class
End Namespace