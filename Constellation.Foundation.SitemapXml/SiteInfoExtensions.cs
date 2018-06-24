using Sitecore.Web;

namespace Constellation.Foundation.SitemapXml
{
	public static class SiteInfoExtensions
	{
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
