Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp.Web.Editors

Namespace WinWebSolution.Module
	Public Class WebSchedulerFilterResourcesListViewController
		Inherits SchedulerActivityListViewController
		Protected Overrides Function GetResources(ByVal resourcesDataSource As Object) As XPCollection
			Return TryCast((CType(resourcesDataSource, WebDataSource)).Collection, XPCollection)
		End Function
	End Class
End Namespace