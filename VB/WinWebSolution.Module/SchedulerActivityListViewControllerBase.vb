Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp.Utils
Imports DevExpress.ExpressApp.Scheduler

Namespace WinWebSolution.Module
	Public MustInherit Class SchedulerActivityListViewControllerBase
		Inherits ViewController(Of ListView)
		Private Shared masterDetailViewEmployeeIdCore As Object
		Private schedulerListEditorCore As SchedulerListEditorBase
		Public Sub New()
			TargetObjectType = GetType(Activity)
		End Sub
		Protected Overrides Overloads Sub OnActivated()
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
				End If
			End If
			schedulerListEditorCore = TryCast((CType(View, ListView)).Editor, SchedulerListEditorBase)
			If schedulerListEditorCore IsNot Nothing Then
				AddHandler schedulerListEditorCore.ResourceDataSourceCreated, AddressOf schedulerListEditorCore_ResourceDataSourceCreated
			End If
		End Sub
		Protected Overrides Overloads Sub OnDeactivating()
			If schedulerListEditorCore IsNot Nothing Then
				RemoveHandler schedulerListEditorCore.ResourceDataSourceCreated, AddressOf schedulerListEditorCore_ResourceDataSourceCreated
			End If
			If (Not View.IsRoot) Then
				Dim propertyCollectionSource As PropertyCollectionSource = TryCast(View.CollectionSource, PropertyCollectionSource)
				If propertyCollectionSource IsNot Nothing Then
					RemoveHandler propertyCollectionSource.MasterObjectChanged, AddressOf propertyCollectionSource_MasterObjectChanged
				End If
			End If
			MyBase.OnDeactivating()
		End Sub
		Private Sub propertyCollectionSource_MasterObjectChanged(ByVal sender As Object, ByVal e As EventArgs)
			Dim masterDetailViewEmployee As Employee = TryCast((CType(sender, PropertyCollectionSource)).MasterObject, Employee)
			If masterDetailViewEmployee IsNot Nothing Then
				masterDetailViewEmployeeIdCore = masterDetailViewEmployee.Oid
				FilterActivities(GetActivitiesFilter(masterDetailViewEmployee))
			End If
		End Sub
		Private Sub schedulerListEditorCore_ResourceDataSourceCreated(ByVal sender As Object, ByVal e As ResourceDataSourceCreatedEventArgs)
			SortResources(e.DataSource)
			FilterResources(e.DataSource, GetResourcesFilter(GetEmployeeById(masterDetailViewEmployeeIdCore)))
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
		Protected MustOverride Sub SortResources(ByVal resourcesDataSource As Object)
		Protected MustOverride Sub FilterResources(ByVal resourcesDataSource As Object, ByVal criteria As CriteriaOperator)
	End Class
End Namespace