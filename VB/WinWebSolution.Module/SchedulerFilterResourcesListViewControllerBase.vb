Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp.Utils
Imports DevExpress.ExpressApp.Scheduler

Namespace WinWebSolution.Module
	Public MustInherit Class SchedulerFilterResourcesListViewControllerBase
		Inherits ViewController
		Private masterDetailViewEmployeeId As Object = Nothing
		Private schedulerListEditorCore As SchedulerListEditorBase
		Public Sub New()
			TargetObjectType = GetType(Activity)
			TargetViewType = ViewType.ListView
		End Sub
		Public ReadOnly Property ListView() As ListView
			Get
				Return CType(View, ListView)
			End Get
		End Property
		Private Sub FilterActivities(ByVal criteria As CriteriaOperator)
			ListView.CollectionSource.Criteria("FilterActivitiesByEmployee") = criteria
		End Sub
		Protected Overrides Sub OnActivated()
			MyBase.OnActivated()
			If (Not IsAdmininstator(SecuritySystem.CurrentUserId)) Then
				If View.IsRoot Then
					FilterActivities(CriteriaOperator.Parse("Employees[Oid = ?]", SecuritySystem.CurrentUserId))
				End If
			End If
			If (Not View.IsRoot) Then
				AddHandler (CType(ListView.CollectionSource, PropertyCollectionSource)).MasterObjectChanged, AddressOf OnPropertyCollectionSourceMasterObjectChanged
			End If
			schedulerListEditorCore = TryCast((CType(View, ListView)).Editor, SchedulerListEditorBase)
			If schedulerListEditorCore IsNot Nothing Then
				AddHandler schedulerListEditorCore.ResourceDataSourceCreated, AddressOf OnSchedulerListEditorResourceDataSourceCreated
			End If
		End Sub
Private Sub OnPropertyCollectionSourceMasterObjectChanged(ByVal sender As Object, ByVal e As EventArgs)
			Dim masterDetailViewEmployee As Employee = (CType((CType(sender, PropertyCollectionSource)).MasterObject, Employee))
			masterDetailViewEmployeeId = masterDetailViewEmployee.Oid
			If (Not IsAdmininstator(SecuritySystem.CurrentUserId)) Then
				If (View.ObjectSpace.IsNewObject(masterDetailViewEmployee) AndAlso IsAdmininstator(masterDetailViewEmployee)) OrElse ((Not View.ObjectSpace.IsNewObject(masterDetailViewEmployee)) AndAlso IsAdmininstator(masterDetailViewEmployeeId)) Then
					FilterActivities(CollectionSource.EmptyCollectionCriteria)
				End If
			Else
				FilterActivities(CriteriaOperator.Parse("Employees[Oid = ?]", masterDetailViewEmployeeId))
			End If
End Sub
		Protected Overrides Sub OnDeactivating()
			If schedulerListEditorCore IsNot Nothing Then
				RemoveHandler schedulerListEditorCore.ResourceDataSourceCreated, AddressOf OnSchedulerListEditorResourceDataSourceCreated
			End If
			MyBase.OnDeactivating()
		End Sub
		Private Sub OnSchedulerListEditorResourceDataSourceCreated(ByVal sender As Object, ByVal e As ResourceDataSourceCreatedEventArgs)
			If (Not IsAdmininstator(SecuritySystem.CurrentUserId)) Then
				If (Not View.IsRoot) Then
					If IsAdmininstator(masterDetailViewEmployeeId) Then
						FilterResourcesDataSource(e.DataSource, CollectionSource.EmptyCollectionCriteria)
					Else
						FilterResourcesDataSource(e.DataSource, CriteriaOperator.Parse("Oid = ?", masterDetailViewEmployeeId))
					End If
				ElseIf View.IsRoot Then
					FilterResourcesDataSource(e.DataSource, CriteriaOperator.Parse("Oid = ?", SecuritySystem.CurrentUserId))
				End If
			End If
			If IsAdmininstator(SecuritySystem.CurrentUserId) AndAlso (Not View.IsRoot) Then
				FilterResourcesDataSource(e.DataSource, CriteriaOperator.Parse("Oid = ?", masterDetailViewEmployeeId))
			End If
		End Sub
		Protected MustOverride Sub FilterResourcesDataSource(ByVal resourcesDataSource As Object, ByVal criteria As CriteriaOperator)
		Private Function IsAdmininstator(ByVal employee As Employee) As Boolean
			Guard.ArgumentNotNull(employee, "employee")
			Return Convert.ToBoolean(employee.Evaluate(CriteriaOperator.Parse("Groups[Name = ?].Count() > 0", Group.DefaultAdministratorsGroupName)))
		End Function
		Private Function IsAdmininstator(ByVal employeeId As Object) As Boolean
			If employeeId Is Nothing Then
				Return False
			End If
			Dim objectSpaceCore As ObjectSpace = Application.CreateObjectSpace()
			Dim obj As Employee = objectSpaceCore.GetObjectByKey(Of Employee)(employeeId)
			Return IsAdmininstator(obj)
		End Function
	End Class
End Namespace