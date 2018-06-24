using Constellation.Foundation.SitemapXml.Nodes;
using Sitecore.Web;

namespace Constellation.Foundation.SitemapXml.Crawlers
{
	public class DefaultContentTreeCrawler : BasicContentTreeCrawler<DefaultSitemapNode>
	{
		public DefaultContentTreeCrawler(SiteInfo site) : base(site)
		{
		}
	}
}
