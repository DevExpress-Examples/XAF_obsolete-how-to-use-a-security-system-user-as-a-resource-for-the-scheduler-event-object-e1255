using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;

namespace WinWebSolution.Module {
    public class WebSchedulerFilterResourcesListViewController : SchedulerFilterResourcesListViewControllerBase {
        protected override void FilterResourcesDataSource(object resourcesDataSource, CriteriaOperator criteria) {
            if (resourcesDataSource is XpoDataSource) {
                ((XpoDataSource)resourcesDataSource).Criteria = criteria.ToString();
            }
        }
    }
}