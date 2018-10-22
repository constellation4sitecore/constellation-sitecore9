using System.Collections.Generic;
using Constellation.Feature.Navigation.Models;
using Constellation.Foundation.Data;
using Constellation.Foundation.ModelMapping;
using Sitecore.Data.Items;
using Sitecore.Web;

namespace Constellation.Feature.Navigation.Repositories
{
	/// <summary>
	/// Generates a list of links that can be used to produce a "breadcrumbs" UX component.
	/// </summary>
	public class BreadcrumbNavigationRepository : IBreadcrumbNavigationRepository
	{
		#region Constructor
		/// <summary>
		/// Creates a new instance of BreadcrumbNavigationRepository
		/// </summary>
		/// <param name="modelMapper">The instance of IModelMapper to use for mapping Item fields to model properties, usually provided by dependency injection.</param>
		public BreadcrumbNavigationRepository(IModelMapper modelMapper)
		{
			ModelMapper = modelMapper;
		}
		#endregion

		#region Properties
		/// <summary>
		/// The instance of IModelMapper to use for mapping Item fields to model properties.
		/// </summary>
		protected IModelMapper ModelMapper { get; }
		#endregion

		/// <summary>
		/// Returns an array of Breadcrumb objects that can be used to generate "breadcrumb" HTML.
		/// </summary>
		/// <param name="contextItem">The Item representing the current page being viewed. (Context Item)</param>
		/// <param name="site">The site being viewed. (Context Site)</param>
		/// <returns>An array of Breadcrumb objects.</returns>
		public Breadcrumb[] GetNavigation(Item contextItem, SiteInfo site)
		{
			var breadcrumbs = new List<Breadcrumb>();
			var ancestors = contextItem.Axes.GetAncestors();
			var homeItem = contextItem.Database.GetItem(site.StartItem);

			foreach (var ancestor in ancestors)
			{
				if (!ancestor.Axes.IsDescendantOf(homeItem))
				{
					continue; // This trims the tree above the site's starting Item node.
				}

				if (!ancestor.IsDerivedFrom(NavigationTemplateIDs.PageID))
				{
					continue; // We only list Pages in breadcrumbs, since they're clickable.
				}

				var breadcrumb = ModelMapper.MapItemToNew<Breadcrumb>(ancestor);
				breadcrumbs.Add(breadcrumb);

				if (breadcrumb.ID.Equals(contextItem.ID))
				{
					breadcrumb.IsContextItem = true;
				}
			}

			return breadcrumbs.ToArray();
		}
	}
}
