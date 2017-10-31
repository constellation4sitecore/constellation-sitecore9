using Sitecore;

namespace Constellation.Foundation.Caching
{
	/// <summary>
	/// Cache Key Generation System
	/// </summary>
	internal class KeyAgent
	{
		/// <summary>
		/// The base site key
		/// </summary>
		private const string SiteFlag = "{0}-{1}-sitecache";

		/// <summary>
		/// The global key
		/// </summary>
		private const string GlobalFlag = "global";

		/// <summary>
		/// Non publish key
		/// </summary>
		internal const string KeepAfterPublishFlag = "keeponpublish";

		#region Internal Methods

		/// <summary>
		/// Gets the base key used for the both sitecore caching and the HttpRuntime cache. If no parameters are
		/// specified, attempts to use the Sitecore Context to resolve the site name and the database name.
		/// </summary>
		/// <param name="isGlobal">True if the cached object should be cached for all sites and databases.</param>
		/// <param name="siteName">The name of the site if the cached object is scoped to a site.</param>
		/// <param name="databaseName">THe name of the database if the cached object is scoped to a database.</param>
		/// <returns></returns>
		internal static string GetBaseKey(bool isGlobal = false, string siteName = "", string databaseName = "")
		{
			// the site name can be overridden
			if (string.IsNullOrEmpty(siteName))
			{
				siteName = Context.Site == null ? "nosite" : Context.Site.Name;
			}

			// the database can be overridden
			if (string.IsNullOrEmpty(databaseName))
			{
				databaseName = Context.Database == null ? "nodb" : Context.Database.Name;
			}

			// are we on the global cache
			return string.Format(SiteFlag, isGlobal ? GlobalFlag : siteName, databaseName).ToLower();
		}

		/// <summary>
		/// Takes an existing key and adds a flag that prevents the object from being removed from cache after publishing.
		/// </summary>
		/// <param name="key">The cache key for the object to be cached.</param>
		/// <returns>A key with a flag to prevent de-caching after publication.</returns>
		internal static string AddKeepAfterPublish(string key)
		{
			return $"{KeepAfterPublishFlag}-{key}";
		}

		#endregion
	}
}
