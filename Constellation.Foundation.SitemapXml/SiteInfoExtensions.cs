using Sitecore.Web;

namespace Constellation.Foundation.SitemapXml
{
	/// <summary>
	/// Provides easy access to sitemap.xml specific attributes added to Sitecore's site definitions.
	/// </summary>
	public static class SiteInfoExtensions
	{
		/// <summary>
		/// The value of the "sitemapXmlCacheTimeout" attribute on a given site element in the Sitecore config.
		/// </summary>
		/// <param name="site">the Site to inspect</param>
		/// <returns>Cache time in minutes, if configured.</returns>
		public static int GetSitemapXmlCacheTimeout(this SiteInfo site)
		{
			if (int.TryParse(site.Properties["sitemapXmlCacheTimeout"], out var cacheTime))
			{
				return cacheTime;
			}

			return SitemapXmlConfiguration.Current.DefaultCacheTimeout;
		}
	}
}
