using System;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Scheduler;

namespace WinWebSolution.Module {
    public abstract class SchedulerActivityListViewControllerBase : ViewController<ListView> {
        private static object masterDetailViewEmployeeIdCore;
        private SchedulerListEditorBase schedulerListEditorCore;
        public SchedulerActivityListViewControllerBase() {
            TargetObjectType = typeof(Activity);
        }
        protected override void OnActivated() {
            base.OnActivated();
            if (!IsAdmininstator(SecuritySystem.CurrentUserId)) {
                if (View.IsRoot) {
                    FilterActivities(CriteriaOperator.Parse("Employees[Oid = ?]", SecuritySystem.CurrentUserId));
                }
            }
            if (!View.IsRoot) {
                PropertyCollectionSource propertyCollectionSource = View.CollectionSource as PropertyCollectionSource;
                if (propertyCollectionSource != null) {
                    propertyCollectionSource.MasterObjectChanged += propertyCollectionSource_MasterObjectChanged;
                }
            }
            schedulerListEditorCore = ((ListView)View).Editor as SchedulerListEditorBase;
            if (schedulerListEditorCore != null) {
                schedulerListEditorCore.ResourceDataSourceCreated += schedulerListEditorCore_ResourceDataSourceCreated;
            }
        }
        protected override void OnDeactivating() {
            if (schedulerListEditorCore != null) {
                schedulerListEditorCore.ResourceDataSourceCreated -= schedulerListEditorCore_ResourceDataSourceCreated;
            }
            if (!View.IsRoot) {
                PropertyCollectionSource propertyCollectionSource = View.CollectionSource as PropertyCollectionSource;
                if (propertyCollectionSource != null) {
                    propertyCollectionSource.MasterObjectChanged -= propertyCollectionSource_MasterObjectChanged;
                }
            }
            base.OnDeactivating();
        }
        private void propertyCollectionSource_MasterObjectChanged(object sender, EventArgs e) {
            Employee masterDetailViewEmployee = ((PropertyCollectionSource)sender).MasterObject as Employee;
            if (masterDetailViewEmployee != null) {
                masterDetailViewEmployeeIdCore = masterDetailViewEmployee.Oid;
                FilterActivities(GetActivitiesFilter(masterDetailViewEmployee));
            }
        }
        private void schedulerListEditorCore_ResourceDataSourceCreated(object sender, ResourceDataSourceCreatedEventArgs e) {
            SortResources(e.DataSource);
            FilterResources(e.DataSource, GetResourcesFilter(GetEmployeeById(masterDetailViewEmployeeIdCore)));
        }
        private CriteriaOperator GetActivitiesFilter(Employee masterDetailViewEmployee) {
            CriteriaOperator criteria = null;
            if (masterDetailViewEmployee != null) {
                if (!IsAdmininstator(SecuritySystem.CurrentUserId)) {
                    if ((View.ObjectSpace.IsNewObject(masterDetailViewEmployee) && IsAdmininstator(masterDetailViewEmployee))
                            || (!View.ObjectSpace.IsNewObject(masterDetailViewEmployee) && IsAdmininstator(masterDetailViewEmployee))) {
                        criteria = CollectionSource.EmptyCollectionCriteria;
                    }
                }
                else {
                    criteria = CriteriaOperator.Parse("Employees[Oid = ?]", masterDetailViewEmployee.Oid);
                }
            }
            return criteria;
        }
        private CriteriaOperator GetResourcesFilter(Employee masterDetailViewEmployee) {
            CriteriaOperator criteria = null;
            bool isCurrentUserAdmin = IsAdmininstator(SecuritySystem.CurrentUserId);
            bool isViewRoot = View.IsRoot;
            bool isMasterDetailEmployeeNotEmpty = masterDetailViewEmployee != null;
            if (!isCurrentUserAdmin) {
                if (!isViewRoot && isMasterDetailEmployeeNotEmpty) {
                    if (IsAdmininstator(masterDetailViewEmployee)) {
                        criteria = CollectionSource.EmptyCollectionCriteria;
                    }
                    else {
                        criteria = CriteriaOperator.Parse("Oid = ?", masterDetailViewEmployee.Oid);
                    }
                }
                else if (isViewRoot) {
                    criteria = CriteriaOperator.Parse("Oid = ?", SecuritySystem.CurrentUserId);
                }
            }
            if (isCurrentUserAdmin && !isViewRoot && isMasterDetailEmployeeNotEmpty) {
                criteria = CriteriaOperator.Parse("Oid = ?", masterDetailViewEmployee.Oid);
            }
            return criteria;
        }
        private Employee GetEmployeeById(object employeeId) {
            return Application.CreateObjectSpace().GetObjectByKey<Employee>(employeeId);
        }
        private bool IsAdmininstator(object employeeId) {
            return IsAdmininstator(GetEmployeeById(employeeId));
        }
        private static bool IsAdmininstator(Employee employee) {
            Guard.ArgumentNotNull(employee, "employee");
            return Convert.ToBoolean(employee.Evaluate(CriteriaOperator.Parse("Groups[Name = ?].Count() > 0", Group.DefaultAdministratorsGroupName)));
        }
        private void FilterActivities(CriteriaOperator criteria) {
            View.CollectionSource.Criteria["FilterActivitiesByEmployee"] = criteria;
        }
        protected abstract void SortResources(object resourcesDataSource);
        protected abstract void FilterResources(object resourcesDataSource, CriteriaOperator criteria);
    }
}