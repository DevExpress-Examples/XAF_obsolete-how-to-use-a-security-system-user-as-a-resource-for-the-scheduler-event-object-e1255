using DevExpress.Xpo;
using DevExpress.Data.Filtering;

namespace WinWebSolution.Module {
    public class WebSchedulerFilterResourcesListViewController : SchedulerActivityListViewControllerBase {
        protected override void FilterResources(object resourcesDataSource, CriteriaOperator criteria) {
            XpoDataSource xds = resourcesDataSource as XpoDataSource;
            if (xds != null && !ReferenceEquals(criteria, null)) {
                xds.Criteria = criteria.ToString();
            }
        }
        protected override void SortResources(object resourcesDataSource) {
            //It's not possible to sort resources on the datasource level. Please track the following suggestion in this regard: http://www.devexpress.com/Support/Center/ViewIssue.aspx?issueid=S18199.
            //The only way is to provide sorting on the control level.
        }
    }
}