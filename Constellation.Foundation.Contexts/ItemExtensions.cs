namespace Constellation.Foundation.Contexts
{
	using Sitecore.Data.Items;
	using Sitecore.Diagnostics;
	using Sitecore.Sites;

	public static class ItemExtensions
	{
		/// <summary>
		/// Determines if the current item is a child of the root of the supplied site.
		/// </summary>
		/// <param name="item">The item to test.</param>
		/// <param name="site">The candidate site.</param>
		/// <returns>True if item is a descendant of the supplied site, false if not or item is null.</returns>
		public static bool IsNativeToSite(this Item item, SiteContext site)
		{
			if (item == null)
			{
				return false;
			}

			if (site == null)
			{
				Log.Warn("Call to IsNativeToSite was supplied a null site argument. Are you trying to use this call outside of a request context?", item);
				return false;
			}

			if (item.Database.Name != site.Database.Name)
			{
				return false;
			}

			var rootItem = site.Database.Items[site.RootPath];

			if (rootItem == null)
			{
				Log.Warn("Call to IsNativeToSite could not resolve a site root item. Is the site root published?", item);
				return false;
			}

			return rootItem.Axes.IsAncestorOf(item);
		}
	}
}
