namespace Constellation.Foundation.Caching
{
	using System;
	using Sitecore.Caching;


	/// <inheritdoc />
	public class CacheManager : ICacheManager
	{
		/// <inheritdoc />
		public T Get<T>(string key,
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

		/// <inheritdoc />
		public void Add(string key, object cachingData, object cacheTimer,
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

		/// <inheritdoc />
		public void Remove(string key,
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
	}
}
