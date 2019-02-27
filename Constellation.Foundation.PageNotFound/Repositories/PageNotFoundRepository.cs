using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Sites;
using Sitecore.Web;

namespace Constellation.Foundation.PageNotFound.Repositories
{
	/// <summary>
	/// The boundary between this library and the Sitecore API. This class handles calls to Sitecore for
	/// retrieving the "Not Found" page for the supplied context.
	/// </summary>
	public class PageNotFoundRepository
	{
		/// <summary>
		/// Given the context data, return the Item that should be the Context Item for a 404 response.
		/// </summary>
		/// <param name="site">The site to inspect</param>
		/// <param name="language">The language the Page Item should be returned in.</param>
		/// <param name="database">The database to search</param>
		/// <returns>The Item to use as the Context Item for a 404 response or Null.</returns>
		public static Item GetPageNotFoundItem(SiteContext site, Language language, Database database)
		{
			return GetPageNotFoundItem(site.SiteInfo, language, database);
		}

		/// <summary>
		/// Given the context data, return the Item that should be the Context Item for a 404 response.
		/// </summary>
		/// <param name="site">The site to inspect</param>
		/// <param name="language">The language the Page Item should be returned in.</param>
		/// <param name="database">The database to search</param>
		/// <returns>The Item to use as the Context Item for a 404 response, or Null.</returns>
		public static Item GetPageNotFoundItem(SiteInfo site, Language language, Database database)
		{
			var id = site.GetPageNotFoundID();

			if (id.IsNull)
			{
				return null;
			}

			if (!site.EnforceVersionPresence)
			{
				return database.GetItem(id, language);
			}

			using (new EnforceVersionPresenceDisabler())
			{
				return database.GetItem(id, language);
			}
		}
	}
}
