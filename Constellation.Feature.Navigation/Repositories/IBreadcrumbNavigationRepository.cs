using Constellation.Feature.Navigation.Models;
using Sitecore.Data.Items;
using Sitecore.Web;

namespace Constellation.Feature.Navigation.Repositories
{
	public interface IBreadcrumbNavigationRepository
	{
		Breadcrumb[] GetNavigation(Item contextItem, SiteInfo site);
	}
}
