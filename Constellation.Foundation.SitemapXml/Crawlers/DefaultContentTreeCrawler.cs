using Constellation.Foundation.SitemapXml.Nodes;
using Sitecore.Web;

namespace Constellation.Foundation.SitemapXml.Crawlers
{
	/// <summary>
	/// A safe, working example of a ContentTree crawler that will produce a working sitemap.xml document
	/// on most Sitecore installations without further modification.
	/// </summary>
	public class DefaultContentTreeCrawler : BasicContentTreeCrawler<DefaultSitemapNode>
	{
		/// <summary>
		/// Creates a new instance of DefaultContentTreeCrawler
		/// </summary>
		/// <param name="site">The site to crawl.</param>
		public DefaultContentTreeCrawler(SiteInfo site) : base(site)
		{
		}
	}
}
