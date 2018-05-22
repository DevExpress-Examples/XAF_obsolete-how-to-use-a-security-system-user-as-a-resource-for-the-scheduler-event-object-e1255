using System;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Scheduler;

namespace WinWebSolution.Module {
    public abstract class SchedulerFilterResourcesListViewControllerBase : ViewController {
        private object masterDetailViewEmployeeId = null;
        private SchedulerListEditorBase schedulerListEditorCore;
        public SchedulerFilterResourcesListViewControllerBase() {
            TargetObjectType = typeof(Activity);
            TargetViewType = ViewType.ListView;
        }
        public ListView ListView {
            get {
                return (ListView)View;
            }
        }
        private void FilterActivities(CriteriaOperator criteria) {
            ListView.CollectionSource.Criteria["FilterActivitiesByEmployee"] = criteria;
        }
        protected override void OnActivated() {
            base.OnActivated();
            if (!IsAdmininstator(SecuritySystem.CurrentUserId)) {
                if (View.IsRoot) {
                    FilterActivities(CriteriaOperator.Parse("Employees[Oid = ?]", SecuritySystem.CurrentUserId));
                }
            }
            if (!View.IsRoot) {
                ((PropertyCollectionSource)ListView.CollectionSource).MasterObjectChanged += OnPropertyCollectionSourceMasterObjectChanged;
            }
            schedulerListEditorCore = ((ListView)View).Editor as SchedulerListEditorBase;
            if (schedulerListEditorCore != null) {
                schedulerListEditorCore.ResourceDataSourceCreated += new System.EventHandler<ResourceDataSourceCreatedEventArgs>(OnSchedulerListEditorResourceDataSourceCreated);
            }
        }
void OnPropertyCollectionSourceMasterObjectChanged(object sender, EventArgs e) {
            Employee masterDetailViewEmployee = ((Employee)((PropertyCollectionSource)sender).MasterObject);
            masterDetailViewEmployeeId = masterDetailViewEmployee.Oid;
            if (!IsAdmininstator(SecuritySystem.CurrentUserId)) {
                if ((View.ObjectSpace.IsNewObject(masterDetailViewEmployee) && IsAdmininstator(masterDetailViewEmployee)) || (!View.ObjectSpace.IsNewObject(masterDetailViewEmployee) && IsAdmininstator(masterDetailViewEmployeeId))) {
                    FilterActivities(CollectionSource.EmptyCollectionCriteria);
                }
            } else {
                FilterActivities(CriteriaOperator.Parse("Employees[Oid = ?]", masterDetailViewEmployeeId));
            }
        }
        protected override void OnDeactivating() {
            if (schedulerListEditorCore != null) {
                schedulerListEditorCore.ResourceDataSourceCreated -= new System.EventHandler<ResourceDataSourceCreatedEventArgs>(OnSchedulerListEditorResourceDataSourceCreated);
            }
            base.OnDeactivating();
        }
        private void OnSchedulerListEditorResourceDataSourceCreated(object sender, ResourceDataSourceCreatedEventArgs e) {
            if (!IsAdmininstator(SecuritySystem.CurrentUserId)) {
                if (!View.IsRoot) {
                    if (IsAdmininstator(masterDetailViewEmployeeId)) {
                        FilterResourcesDataSource(e.DataSource, CollectionSource.EmptyCollectionCriteria);
                    } else {
                        FilterResourcesDataSource(e.DataSource, CriteriaOperator.Parse("Oid = ?", masterDetailViewEmployeeId));
                    }
                } else if (View.IsRoot) {
                    FilterResourcesDataSource(e.DataSource, CriteriaOperator.Parse("Oid = ?", SecuritySystem.CurrentUserId));
                }
            }
            if (IsAdmininstator(SecuritySystem.CurrentUserId) && !View.IsRoot) {
                FilterResourcesDataSource(e.DataSource, CriteriaOperator.Parse("Oid = ?", masterDetailViewEmployeeId));
            }
        }
        protected abstract void FilterResourcesDataSource(object resourcesDataSource, CriteriaOperator criteria);
        private bool IsAdmininstator(Employee employee) {
            Guard.ArgumentNotNull(employee, "employee");
            return Convert.ToBoolean(employee.Evaluate(CriteriaOperator.Parse("Groups[Name = ?].Count() > 0", Group.DefaultAdministratorsGroupName)));
        }
        private bool IsAdmininstator(object employeeId) {
            if (employeeId == null) return false;
            ObjectSpace objectSpaceCore = Application.CreateObjectSpace();
            Employee obj = objectSpaceCore.GetObjectByKey<Employee>(employeeId);
            return IsAdmininstator(obj);
        }
    }
}