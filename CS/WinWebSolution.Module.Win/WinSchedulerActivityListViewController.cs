using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Data.Filtering;

namespace WinWebSolution.Module {
    public class WinSchedulerFilterResourcesListViewController : SchedulerActivityListViewControllerBase {
        protected override void FilterResources(object resourcesDataSource, CriteriaOperator criteria) {
            XPCollection resources = resourcesDataSource as XPCollection;
            if (resources != null) {
                resources.Criteria = criteria;
            }
        }
        protected override void SortResources(object resourcesDataSource) {
            XPCollection resources = resourcesDataSource as XPCollection;
            if (resources != null) {
                XpoSortingHelper.Sort(resources, "Caption", SortingDirection.Descending);
            }
        }
    }
    public class XpoSortingHelper {
        public static void Sort(XPBaseCollection collection, string property, SortingDirection direction) {
            bool isSortingAdded = false;
            foreach (SortProperty sortProperty in collection.Sorting) {
                if (sortProperty.Property.Equals(CriteriaOperator.Parse(property))) {
                    isSortingAdded = true;
                }
            }
            if (!isSortingAdded) {
                collection.Sorting.Add(new SortProperty(property, direction));
            }
        }
    }
}