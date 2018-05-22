using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;

namespace WinWebSolution.Module {
    public class WinSchedulerFilterResourcesListViewController : SchedulerFilterResourcesListViewControllerBase {
        protected override void FilterResourcesDataSource(object resourcesDataSource, CriteriaOperator criteria) {
            if (resourcesDataSource is XPCollection) {
                ((XPCollection)resourcesDataSource).Criteria = criteria;
            }
        }
    }
}