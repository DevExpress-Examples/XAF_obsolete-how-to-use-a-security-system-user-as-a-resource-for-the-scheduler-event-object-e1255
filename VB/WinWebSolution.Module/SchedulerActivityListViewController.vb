Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Xpo
Imports DevExpress.Data
Imports DevExpress.Xpo.DB
Imports DevExpress.ExpressApp
Imports DevExpress.Data.Filtering
Imports System.Collections.Generic
Imports DevExpress.ExpressApp.Model
Imports DevExpress.ExpressApp.Utils
Imports DevExpress.ExpressApp.Scheduler

Namespace WinWebSolution.Module
	Public Class SchedulerActivityListViewController
		Inherits ViewController(Of ListView)
		Private masterDetailViewEmployeeIdCore As Object
		Private schedulerListEditorCore As SchedulerListEditorBase
		Public Sub New()
			TargetObjectType = GetType(Activity)
		End Sub
		Protected Overrides Sub OnActivated()
			MyBase.OnActivated()
			If (Not IsAdmininstator(SecuritySystem.CurrentUserId)) Then
				If View.IsRoot Then
					FilterActivities(CriteriaOperator.Parse("Employees[Oid = ?]", SecuritySystem.CurrentUserId))
				End If
			End If
			If (Not View.IsRoot) Then
				Dim propertyCollectionSource As PropertyCollectionSource = TryCast(View.CollectionSource, PropertyCollectionSource)
				If propertyCollectionSource IsNot Nothing Then
					AddHandler propertyCollectionSource.MasterObjectChanged, AddressOf propertyCollectionSource_MasterObjectChanged
					UpdateMasterDetailViewEmployee(propertyCollectionSource)
				End If
			End If
			schedulerListEditorCore = TryCast((CType(View, ListView)).Editor, SchedulerListEditorBase)
			If schedulerListEditorCore IsNot Nothing Then
				AddHandler schedulerListEditorCore.ResourceDataSourceCreated, AddressOf schedulerListEditorCore_ResourceDataSourceCreated
			End If
		End Sub
		Protected Overrides Sub OnDeactivated()
			If schedulerListEditorCore IsNot Nothing Then
				RemoveHandler schedulerListEditorCore.ResourceDataSourceCreated, AddressOf schedulerListEditorCore_ResourceDataSourceCreated
			End If
			If (Not View.IsRoot) Then
				Dim propertyCollectionSource As PropertyCollectionSource = TryCast(View.CollectionSource, PropertyCollectionSource)
				If propertyCollectionSource IsNot Nothing Then
					RemoveHandler propertyCollectionSource.MasterObjectChanged, AddressOf propertyCollectionSource_MasterObjectChanged
				End If
			End If
			MyBase.OnDeactivated()
		End Sub
		Private Sub propertyCollectionSource_MasterObjectChanged(ByVal sender As Object, ByVal e As EventArgs)
			UpdateMasterDetailViewEmployee(CType(sender, PropertyCollectionSource))
		End Sub
		Private Sub UpdateMasterDetailViewEmployee(ByVal propertyCollectionSource As PropertyCollectionSource)
			If propertyCollectionSource Is Nothing Then
				Return
			End If
			Dim masterDetailViewEmployee As Employee = TryCast(propertyCollectionSource.MasterObject, Employee)
			If masterDetailViewEmployee IsNot Nothing Then
				masterDetailViewEmployeeIdCore = masterDetailViewEmployee.Oid
				FilterActivities(GetActivitiesFilter(masterDetailViewEmployee))
			End If
		End Sub
		Private Sub schedulerListEditorCore_ResourceDataSourceCreated(ByVal sender As Object, ByVal e As ResourceDataSourceCreatedEventArgs)
			SortResources(GetResources(e.DataSource))
			FilterResources(GetResources(e.DataSource), GetResourcesFilter(GetEmployeeById(masterDetailViewEmployeeIdCore)))
		End Sub
		Private Function GetActivitiesFilter(ByVal masterDetailViewEmployee As Employee) As CriteriaOperator
			Dim criteria As CriteriaOperator = Nothing
			If masterDetailViewEmployee IsNot Nothing Then
				If (Not IsAdmininstator(SecuritySystem.CurrentUserId)) Then
					If (View.ObjectSpace.IsNewObject(masterDetailViewEmployee) AndAlso IsAdmininstator(masterDetailViewEmployee)) OrElse ((Not View.ObjectSpace.IsNewObject(masterDetailViewEmployee)) AndAlso IsAdmininstator(masterDetailViewEmployee)) Then
						criteria = CollectionSource.EmptyCollectionCriteria
					End If
				Else
					criteria = CriteriaOperator.Parse("Employees[Oid = ?]", masterDetailViewEmployee.Oid)
				End If
			End If
			Return criteria
		End Function
		Private Function GetResourcesFilter(ByVal masterDetailViewEmployee As Employee) As CriteriaOperator
			Dim criteria As CriteriaOperator = Nothing
			Dim isCurrentUserAdmin As Boolean = IsAdmininstator(SecuritySystem.CurrentUserId)
			Dim isViewRoot As Boolean = View.IsRoot
			Dim isMasterDetailEmployeeNotEmpty As Boolean = masterDetailViewEmployee IsNot Nothing
			If (Not isCurrentUserAdmin) Then
				If (Not isViewRoot) AndAlso isMasterDetailEmployeeNotEmpty Then
					If IsAdmininstator(masterDetailViewEmployee) Then
						criteria = CollectionSource.EmptyCollectionCriteria
					Else
						criteria = CriteriaOperator.Parse("Oid = ?", masterDetailViewEmployee.Oid)
					End If
				ElseIf isViewRoot Then
					criteria = CriteriaOperator.Parse("Oid = ?", SecuritySystem.CurrentUserId)
				End If
			End If
			If isCurrentUserAdmin AndAlso (Not isViewRoot) AndAlso isMasterDetailEmployeeNotEmpty Then
				criteria = CriteriaOperator.Parse("Oid = ?", masterDetailViewEmployee.Oid)
			End If
			Return criteria
		End Function
		Private Function GetEmployeeById(ByVal employeeId As Object) As Employee
			Return Application.CreateObjectSpace().GetObjectByKey(Of Employee)(employeeId)
		End Function
		Private Function IsAdmininstator(ByVal employeeId As Object) As Boolean
			Return IsAdmininstator(GetEmployeeById(employeeId))
		End Function
		Private Shared Function IsAdmininstator(ByVal employee As Employee) As Boolean
			Guard.ArgumentNotNull(employee, "employee")
			Return Convert.ToBoolean(employee.Evaluate(CriteriaOperator.Parse("Groups[Name = ?].Count() > 0", Group.DefaultAdministratorsGroupName)))
		End Function
		Private Sub FilterActivities(ByVal criteria As CriteriaOperator)
			View.CollectionSource.Criteria("FilterActivitiesByEmployee") = criteria
		End Sub
		Protected Sub SortResources(ByVal resources As XPCollection)
			If resources IsNot Nothing Then
				Dim resourcesListView As IModelListView = Application.FindModelClass(resources.ObjectType).DefaultListView
				XpoSortingHelper.Sort(resources, XpoSortingHelper.GetListViewSorting(resourcesListView))
			End If
		End Sub
		Protected Overridable Sub FilterResources(ByVal resources As XPCollection, ByVal criteria As CriteriaOperator)
			If resources IsNot Nothing AndAlso (Not ReferenceEquals(criteria, Nothing)) Then
				resources.Criteria = criteria
			End If
		End Sub
		Protected Overridable Function GetResources(ByVal resourcesDataSource As Object) As XPCollection
			Return TryCast(resourcesDataSource, XPCollection)
		End Function
	End Class
	Public Class XpoSortingHelper
		Public Shared Function GetListViewSorting(ByVal modelListView As IModelListView) As SortingCollection
			Dim sorting As New List(Of SortProperty)(modelListView.Columns.Count)
			For Each column As IModelColumn In modelListView.Columns
				If column.SortOrder <> ColumnSortOrder.None AndAlso column.SortIndex >= 0 Then
					Dim direction As SortingDirection = SortingDirection.Ascending
					If column.SortOrder = ColumnSortOrder.Descending Then
						direction = SortingDirection.Descending
					End If
					sorting.Insert(column.SortIndex, New SortProperty(column.PropertyName, direction))
				End If
			Next column
			Return New SortingCollection(sorting.ToArray())
		End Function
		Public Shared Sub Sort(ByVal collection As XPBaseCollection, ByVal sorting As SortingCollection)
			collection.Sorting = sorting

		End Sub
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