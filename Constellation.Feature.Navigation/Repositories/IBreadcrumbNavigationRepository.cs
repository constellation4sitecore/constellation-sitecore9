using Constellation.Feature.Navigation.Models;
using Sitecore.Data.Items;
using Sitecore.Web;

namespace Constellation.Feature.Navigation.Repositories
{
	/// <summary>
	/// Generates a list of links that can be used to produce a "breadcrumbs" UX component.
	/// </summary>
	public interface IBreadcrumbNavigationRepository
	{
		/// <summary>
		/// Returns an array of Breadcrumb objects that can be used to generate "breadcrumb" HTML.
		/// </summary>
		/// <param name="contextItem">The Item representing the current page being viewed. (Context Item)</param>
		/// <param name="site">The site being viewed. (Context Site)</param>
		/// <returns>An array of Breadcrumb objects.</returns>
		Breadcrumb[] GetNavigation(Item contextItem, SiteInfo site);
	}
}
