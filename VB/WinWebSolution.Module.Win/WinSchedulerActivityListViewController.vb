Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Xpo
Imports DevExpress.Xpo.DB
Imports DevExpress.Data.Filtering

Namespace WinWebSolution.Module
	Public Class WinSchedulerFilterResourcesListViewController
		Inherits SchedulerActivityListViewControllerBase
		Protected Overrides Sub FilterResources(ByVal resourcesDataSource As Object, ByVal criteria As CriteriaOperator)
			Dim resources As XPCollection = TryCast(resourcesDataSource, XPCollection)
			If resources IsNot Nothing Then
				resources.Criteria = criteria
			End If
		End Sub
		Protected Overrides Sub SortResources(ByVal resourcesDataSource As Object)
			Dim resources As XPCollection = TryCast(resourcesDataSource, XPCollection)
			If resources IsNot Nothing Then
				XpoSortingHelper.Sort(resources, "Caption", SortingDirection.Descending)
			End If
		End Sub
	End Class
	Public Class XpoSortingHelper
		Public Shared Sub Sort(ByVal collection As XPBaseCollection, ByVal [property] As String, ByVal direction As SortingDirection)
			Dim isSortingAdded As Boolean = False
			For Each sortProperty As SortProperty In collection.Sorting
				If sortProperty.Property.Equals(CriteriaOperator.Parse([property])) Then
					isSortingAdded = True
				End If
			Next sortProperty
			If (Not isSortingAdded) Then
				collection.Sorting.Add(New SortProperty([property], direction))
			End If
		End Sub
	End Class
End Namespace