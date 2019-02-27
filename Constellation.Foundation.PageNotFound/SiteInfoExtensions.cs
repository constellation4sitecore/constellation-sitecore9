using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Sites;
using Sitecore.Web;

namespace Constellation.Foundation.PageNotFound
{
	/// <summary>
	/// Extension to specifically deal with the NotFoundPageID custom property.
	/// </summary>
	public static class SiteInfoExtensions
	{
		/// <summary>
		/// Retrieves the value of the notFoundPageID custom site property
		/// </summary>
		/// <param name="site">The SiteContext to inspect</param>
		/// <returns>An ID or null</returns>
		// ReSharper disable once InconsistentNaming
		public static ID GetPageNotFoundID(this SiteContext site)
		{
			return GetPageNotFoundID(site.SiteInfo);
		}

		/// <summary>
		/// Retrieves the value of the notFoundPageID custom site property
		/// </summary>
		/// <param name="site">The SiteInfo to inspect</param>
		/// <returns>An ID or null</returns>
		// ReSharper disable once InconsistentNaming
		public static ID GetPageNotFoundID(this SiteInfo site)
		{
			string value = site.Properties["notFoundPageID"];

			if (!string.IsNullOrEmpty(value))
			{
				if (ID.TryParse(value, out ID id))
				{
					return id;
				}
			}

			Log.Warn($"Constellation.Foundation.PageNotFound SiteInfoExtensions: No value for notFoundPageID on site {site.Name}", typeof(SiteInfoExtensions));
			return null;
		}
	}
}
