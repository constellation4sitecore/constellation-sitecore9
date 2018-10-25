using Sitecore.DependencyInjection;

namespace Constellation.Foundation.Caching
{
	/// <summary>
	/// Retrieves the CacheManager from the Service Provider.
	/// </summary>
	public static class CachingContext
	{
		/// <summary>
		/// The CacheManager to use for the given scope.
		/// </summary>
		public static ICacheManager Current
		{
			get { return (ICacheManager)ServiceLocator.ServiceProvider.GetService(typeof(ICacheManager)); }
		}
	}
}
