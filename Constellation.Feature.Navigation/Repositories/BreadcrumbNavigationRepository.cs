using System.Collections.Generic;
using Constellation.Feature.Navigation.Models;
using Constellation.Foundation.Data;
using Constellation.Foundation.ModelMapping;
using Sitecore.Data.Items;
using Sitecore.Web;

namespace Constellation.Feature.Navigation.Repositories
{
	public class BreadcrumbNavigationRepository : IBreadcrumbNavigationRepository
	{
		#region Constructor
		public BreadcrumbNavigationRepository(IModelMapper modelMapper)
		{
			ModelMapper = modelMapper;
		}
		#endregion

		#region Properties
		protected IModelMapper ModelMapper { get; }
		#endregion

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
