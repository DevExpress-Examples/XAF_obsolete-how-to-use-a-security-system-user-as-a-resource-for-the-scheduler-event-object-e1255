Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering

Namespace WinWebSolution.Module
	Public Class WebSchedulerFilterResourcesListViewController
		Inherits SchedulerActivityListViewControllerBase
		Protected Overrides Sub FilterResources(ByVal resourcesDataSource As Object, ByVal criteria As CriteriaOperator)
			Dim xds As XpoDataSource = TryCast(resourcesDataSource, XpoDataSource)
			If xds IsNot Nothing AndAlso (Not ReferenceEquals(criteria, Nothing)) Then
				xds.Criteria = criteria.ToString()
			End If
		End Sub
		Protected Overrides Sub SortResources(ByVal resourcesDataSource As Object)
			'It's not possible to sort resources on the datasource level. Please track the following suggestion in this regard: http://www.devexpress.com/Support/Center/ViewIssue.aspx?issueid=S18199.
			'The only way is to provide sorting on the control level.
		End Sub
	End Class
End Namespace