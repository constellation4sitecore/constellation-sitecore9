using System.Collections.Generic;
using Sitecore.Caching;
using Sitecore.Configuration;

namespace Constellation.Foundation.Caching
{
	/// <summary>
	/// Provides a facade for retrieving (and creating) a scoped cache
	/// using Sitecore's Cache API.
	/// </summary>
	/// <remarks>
	/// Using GetCache will either return an existing Cache from Sitecore
	/// or create a new one based upon the scoping parameters supplied.
	/// Using ClearCache will attempt to clear a cache within the scoping
	/// parameters supplied. If the cache doesn't exist, nothing happens.
	/// </remarks>
	internal class SitecoreCacheManager
	{
		#region Locals
		/// <summary>
		/// The locking approach to keep the system from throwing threading issues
		/// </summary>
		private static readonly object CacheLock = new object();

		/// <summary>
		/// The caching collection singleton to hold the caching references
		/// </summary>
		private static readonly Dictionary<string, ICache> CacheCollection = new Dictionary<string, ICache>();
		#endregion

		/// <summary>
		/// Fetches a cache from Sitecore with the provided scope parameters.
		/// </summary>
		/// <param name="isGlobal">Is the</param>
		/// <param name="siteName">Name of the site.</param>
		/// <param name="databaseName">Name of the database.</param>
		/// <returns>Returns the sitecore cache instance</returns>
		public static ICache GetCache(bool isGlobal = false, string siteName = "", string databaseName = "")
		{
			string key = KeyAgent.GetBaseKey(isGlobal, siteName, databaseName);

			ICache cache;

			// we need to lock the cache due to multi threaded features
			lock (CacheLock)
			{
				if (CacheCollection.ContainsKey(key))
				{
					// use the cache from the collection
					cache = CacheCollection[key];
				}
				else
				{
					// Try to get it from Sitecore or make a new one.
					//cache = ICache.FindCacheByName(key) ?? new Sitecore.Caching.Cache(key, Settings.GetSetting("Caching.CacheSize", "100MB"));

					cache = Sitecore.Caching.CacheManager.GetNamedInstance(key, Sitecore.StringUtil.ParseSizeString(Settings.GetSetting("Caching.CacheSize", "100MB")), true);
				}
			}

			return cache;
		}

		#region ClearCache

		/// <summary>
		/// Clears the cache with the scope of the parameters supplied.
		/// </summary>
		/// <param name="siteName">The name of the site that needs its cache cleared</param>
		/// <param name="databaseName">The name of the database that needs its cache cleared</param>
		/// <param name="globalCache">Is it a global cache?</param>
		/// <param name="forceRemove">Remove data that is explicitly marked as "do not remove during a publish"</param>
		public static void ClearCache(
			string siteName = "",
			string databaseName = "",
			bool globalCache = false,
			bool forceRemove = false)
		{
			// get the cache from sitecore
			ICache cache = GetCache(globalCache, siteName, databaseName);


			if (cache == null)
			{
				return;
			}

			foreach (var key in cache.GetCacheKeys())
			{
				if (key.Contains(KeyAgent.KeepAfterPublishFlag) && !forceRemove)
				{
					continue;
				}

				cache.Remove(key);
			}

		}

		#endregion
	}
}
