namespace Constellation.Foundation.Caching
{
	using System;
	using Sitecore.Caching;

	/// <summary>
	/// A facade for accessing caching in both the Sitecore API as well as System.Web
	/// </summary>
	/// <remarks>
	/// This cache class provides the typical Add/Remove features of cache APIs, but 
	/// allows you to specify the scope of the cached object, allowing for cached
	/// variants based on site, database, explicit global scope, and cache API.
	/// 
	/// An updated and refactored version of Christopher Giddings' code off his blog.
	/// Here's the link: 
	/// https://cjgiddings.wordpress.com/2012/02/03/sitecore-caching-utility/ 
	/// </remarks>
	public class Cache
	{
		#region Methods
		/// <summary>
		/// Attempts to retrieve an object from the specified cache.
		/// </summary>
		/// <typeparam name="T">The type of parameter to cast the object to</typeparam>
		/// <param name="key">The cache key we are looking for</param>
		/// <param name="useSitecoreCache">Look in the Sitecore cache or the HttpRuntime cache?</param>
		/// <param name="globalCache">Is the item globally cached?</param>
		/// <param name="siteName">The site name</param>
		/// <param name="databaseName">The database name</param>
		/// <returns>Returns the cached data or null</returns>
		public static T Get<T>(string key,
			bool useSitecoreCache = true,
			bool globalCache = false,
			string siteName = "",
			string databaseName = "")
		{
			// no key so return error
			if (string.IsNullOrEmpty(key))
			{
				return default(T);
			}

			// setup the default object we will use
			var cachedItem = default(T);

			// what type of cache are we using
			if (useSitecoreCache)
			{
				// get the cache we are looking for
				ICache cache = SitecoreCacheManager.GetCache(globalCache, siteName, databaseName);

				if (cache != null)
				{
					// make sure the system has the key before doing anything
					if (cache.ContainsKey(key))
					{
						// get the data from the sitecore cache
						cachedItem = (T)cache[key];
					}
					else
					{
						// the item might be a non publish key
						key = KeyAgent.AddKeepAfterPublish(key);

						// get the data if found
						if (cache.ContainsKey(key))
						{
							// get the data from the sitecore cache
							cachedItem = (T)cache[key];
						}
					}
				}
			}
			else
			{
				// set the cache key
				var globalBaseKey = KeyAgent.GetBaseKey(globalCache, siteName, databaseName);
				var cacheStartKey = globalBaseKey + key;

				// get the cache item from the http runtime
				cachedItem = (T)System.Web.HttpRuntime.Cache.Get(cacheStartKey.ToLower());

				// is empty try a non-published item
				if (cachedItem == null)
				{
					// the item might be a non publish key
					cacheStartKey = globalBaseKey + KeyAgent.AddKeepAfterPublish(key);

					// get the non publish clearing item
					cachedItem = (T)System.Web.HttpRuntime.Cache.Get(cacheStartKey);
				}
			}

			return cachedItem;
		}

		/// <summary>
		/// Adds the supplied object to the cache.
		/// </summary>
		/// <param name="key">The unique key to save</param>
		/// <param name="cachingData">The data to cache</param>
		/// <param name="cacheTimer">Either a DateTime when the value should expire or a TimeSpan</param>
		/// <param name="isNoSlidingExpiration">Is the cacheTimer an Absolute Expiration (default) or a sliding expiration</param>
		/// <param name="useSitecoreCache">Do you want to use Sitecore cache or the HttpRuntime cache object</param>
		/// <param name="globalCache">Is the data to be stored in the global cache or site specific cache</param>
		/// <param name="removeOnPublish">Remove the contents on a publish, this is defaulted as true</param>
		/// <param name="siteName">Force set the site name, if this is run from a scheduled task this should be set</param>
		/// <param name="databaseName">Force the database if this is run from a scheduled tasks, this should be set</param>
		/// <param name="cacheDep">Any caching dependencies for the cache. NOTE: Not valid for Sitecore Caching</param>
		/// <param name="priority">The priority of the cache. NOTE: Not valid for Sitecore Caching</param>
		/// <param name="callback">The call-back function of the cache. NOTE: Not valid for Sitecore Caching</param>
		public static void Add(string key, object cachingData, object cacheTimer,
			bool isNoSlidingExpiration = true,
			bool useSitecoreCache = true,
			bool globalCache = false,
			bool removeOnPublish = true,
			string siteName = "",
			string databaseName = "",
			System.Web.Caching.CacheDependency cacheDep = null,
			System.Web.Caching.CacheItemPriority priority = System.Web.Caching.CacheItemPriority.Normal,
			System.Web.Caching.CacheItemRemovedCallback callback = null)
		{
			// make sure we have data
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException(nameof(key));
			}

			if (cachingData == null)
			{
				throw new ArgumentNullException(nameof(cachingData));
			}

			if (!removeOnPublish)
			{
				key = KeyAgent.AddKeepAfterPublish(key);
			}

			// setup defaults for caching types
			TimeSpan slidingCache = System.Web.Caching.Cache.NoSlidingExpiration;
			DateTime absoluteCache = System.Web.Caching.Cache.NoAbsoluteExpiration;

			// set the cache type
			if (isNoSlidingExpiration)
			{
				// make sure it's right
				if (cacheTimer is DateTime)
				{
					absoluteCache = (DateTime)cacheTimer;
				}
				else
				{
					// we have an issue fix up
					var timeSpanCheck = (TimeSpan)cacheTimer;
					absoluteCache = DateTime.Now.Add(timeSpanCheck);
				}
			}
			else
			{
				// make sure it's right
				if (cacheTimer is TimeSpan)
				{
					slidingCache = (TimeSpan)cacheTimer;
				}
				else
				{
					// we have an issue fix up
					var dateCheck = (DateTime)cacheTimer;
					slidingCache = dateCheck.Subtract(DateTime.Now);
				}
			}

			// what type of cache are we using
			if (useSitecoreCache)
			{

				ICache cache = SitecoreCacheManager.GetCache(globalCache, siteName, databaseName);

				if (cache.ContainsKey(key))
				{
					cache.Remove(key);
				}

				cache.Add(key, cachingData, slidingCache, absoluteCache);
			}
			else
			{
				var cacheStartKey = KeyAgent.GetBaseKey(globalCache, siteName, databaseName) + key;

				System.Web.HttpRuntime.Cache.Add(cacheStartKey, cachingData, cacheDep, absoluteCache, slidingCache, priority, callback);
			}
		}
		#endregion

		#region RemoveCacheItem

		/// <summary>
		/// Removes the specified object from the cache
		/// </summary>
		/// <param name="key">The cache key</param>
		/// <param name="useSitecoreCache">Do we want to use the cache</param>
		/// <param name="globalCache">Are we using the global cache</param>
		/// <param name="siteName">The site name</param>
		/// <param name="databaseName">The database name</param>
		/// <returns>Returns true if the data was removed from the cache or false if it wasnt or there was an error</returns>
		public static void Remove(string key,
			bool useSitecoreCache = true,
			bool globalCache = false,
			string siteName = "",
			string databaseName = "")
		{
			// no key so return error
			if (string.IsNullOrEmpty(key))
			{
				return;
			}

			// what type of cache are we using
			if (useSitecoreCache)
			{
				// get the cache we are looking for
				ICache cache = SitecoreCacheManager.GetCache(globalCache, siteName, databaseName);

				if (cache != null)
				{
					// make sure the system has the key before doing anything
					if (cache.ContainsKey(key))
					{
						// remove the cached object
						cache.Remove(key);
					}
					else
					{
						// the item might be a non publish key
						key = KeyAgent.AddKeepAfterPublish(key);

						// get the data if found
						if (cache.ContainsKey(key))
						{
							// remove the cached object
							cache.Remove(key);
						}
					}
				}
			}
			else
			{
				// set the cache key
				var globalBaseKey = KeyAgent.GetBaseKey(globalCache, siteName, databaseName);
				var cacheStartKey = globalBaseKey + key;

				// removes the cache key
				System.Web.HttpRuntime.Cache.Remove(cacheStartKey);

				// remove the non publish key
				System.Web.HttpRuntime.Cache.Remove(globalBaseKey + KeyAgent.AddKeepAfterPublish(key));
			}
		}
		#endregion
	}
}
