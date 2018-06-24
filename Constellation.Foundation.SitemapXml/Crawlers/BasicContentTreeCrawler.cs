using System;
using System.Collections.Generic;
using System.Text;
using Constellation.Foundation.SitemapXml.Nodes;
using Sitecore.Data;
using Sitecore.Data.Query;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Sites;
using Sitecore.Web;

namespace Constellation.Foundation.SitemapXml.Crawlers
{
	public class BasicContentTreeCrawler<T> : Crawler
	where T : ItemBasedSitemapNode, new()
	{
		public BasicContentTreeCrawler(SiteInfo site) : base(site)
		{
		}

		protected override ICollection<ISitemapNode> GetNodes(SiteInfo site, Database database, Language language)
		{
			var output = new List<ISitemapNode>();

			/*
			 *  We're going to crawl the site layer-by-layer which will put the upper levels
			 *  of the site nearer the top of the sitemap.xml document as opposed to crawling
			 *  the tree by parent/child relationships, which will go deep on each branch before
			 *  crawling the entire site.
			 */
			var max = Sitecore.Configuration.Settings.MaxTreeDepth;
			var siteContext = new SiteContext(site);

			var root = Database.GetItem(siteContext.StartPath, language);

			if (root == null)
			{
				var ex = new Exception($"Root item {siteContext.StartPath} was null.");
				Log.Error($"Constellation.Foundation.SitemapXml DefaultCrawler: {ex.Message} ", ex, this);
				throw ex;
			}

			var rootNode = ItemBasedSitemapNode.Create<T>(site, root);
			if (!rootNode.IsValidForInclusionInSitemapXml())
			{
				//Nothing to run, we have to stop here.
				return output;
			}

			output.Add(rootNode);

			var path = new StringBuilder("./*");

			for (var i = 0; i < max; i++)
			{
				var items = Query.SelectItems(path.ToString(), root);

				if (items != null)
				{
					foreach (var item in items)
					{
						var node = ItemBasedSitemapNode.Create<T>(site, item);

						if (node.IsValidForInclusionInSitemapXml())
						{
							output.Add(node);
						}
					}
				}

				path.Append("/*");
			}

			return output;
		}


	}
}
