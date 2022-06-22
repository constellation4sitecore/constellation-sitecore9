using Sitecore.Sites;
using Sitecore.Web;
using System.Xml;

namespace Constellation.Foundation.SitemapXml.Repositories
{
	/// <summary>
	/// Provides access to retrieving an XmlDocument for the supplied Site.
	/// </summary>
	public class SitemapRepository
	{
		/// <summary>
		/// Used to determine if the Site should be crawled and a sitemap.xml should be generated.
		/// </summary>
		/// <param name="site">The site to inspect.</param>
		/// <returns>true if the configuration files indicate this site should be ignored.</returns>
		public static bool IsOnIgnoreList(SiteContext site)
		{
			return IsOnIgnoreList(site.SiteInfo);
		}

		/// <summary>
		/// Used to determine if the Site should be crawled and a sitemap.xml should be generated.
		/// </summary>
		/// <param name="site">The site to inspect.</param>
		/// <returns>true if the configuration files indicate this site should be ignored.</returns>
		public static bool IsOnIgnoreList(SiteInfo site)
		{
			return SitemapXmlConfiguration.Current.SitesToIgnore.Contains(site.Name);
		}

		/// <summary>
		/// Returns a sitemap.xml document string for the supplied site.
		/// </summary>
		/// <param name="site">The site to crawl.</param>
		/// <param name="path">The Url of the sitemap.xml request. This potentially allows for multiple sitemap.xml files to be generated per site.</param>
		/// <param name="forceRegenerate">Whether the document can be retrieved from cache, or whether a fresh document should be generated.</param>
		/// <returns>An XML string representing sitemap.xml document.</returns>
		public static string GetSitemap(SiteContext site, string path, bool forceRegenerate = false)
		{

			var key = typeof(SitemapRepository).FullName + site.Name + path;
			var cache = Sitecore.Caching.CacheManager.GetHtmlCache(site);
			string output = null;

			if (cache != null)
			{
				output = cache.GetHtml(key);
			}

			if (SitemapXmlConfiguration.Current.CacheEnabled && cache != null)
			{
				output = cache.GetHtml(key);
			}

			if (forceRegenerate || !SitemapXmlConfiguration.Current.CacheEnabled || output == null)
			{
				var generator = new SitemapGenerator(site.SiteInfo);

				XmlDocument document = generator.Generate();
				output = document.OuterXml;


				if (SitemapXmlConfiguration.Current.CacheEnabled && cache != null)
				{
					cache.SetHtml(key, output);
				}
			}

			return output;
		}
	}
}
