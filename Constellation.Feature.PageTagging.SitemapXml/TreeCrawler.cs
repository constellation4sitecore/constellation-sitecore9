using Constellation.Foundation.SitemapXml.Crawlers;
using Sitecore.Web;

namespace Constellation.Feature.PageTagging.SitemapXml
{
	/// <summary>
	/// A sitemap.xml Crawler that takes advantage of the Page Search Engine Directives template to determine
	/// if a page should be listed in the sitemap.xml
	/// </summary>
	public class TreeCrawler : BasicContentTreeCrawler<SitemapNode>
	{
		/// <summary>
		/// Creates a new instance of TreeCrawler
		/// </summary>
		/// <param name="site">The site to crawl.</param>
		public TreeCrawler(SiteInfo site) : base(site)
		{
		}
	}
}
