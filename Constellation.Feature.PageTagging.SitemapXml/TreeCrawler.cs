using Constellation.Foundation.SitemapXml.Crawlers;
using Sitecore.Web;

namespace Constellation.Feature.PageTagging.SitemapXml
{
	public class TreeCrawler : BasicContentTreeCrawler<SitemapNode>
	{
		public TreeCrawler(SiteInfo site) : base(site)
		{
		}
	}
}
