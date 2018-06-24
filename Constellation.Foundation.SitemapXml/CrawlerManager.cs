using System;
using System.Collections.Generic;
using Constellation.Foundation.SitemapXml.Crawlers;
using Constellation.Foundation.SitemapXml.Nodes;
using Sitecore.Diagnostics;
using Sitecore.Web;

namespace Constellation.Foundation.SitemapXml
{
	public class CrawlerManager
	{

		public SiteInfo Site { get; private set; }

		public ICollection<Type> Crawlers { get; private set; }


		public CrawlerManager(SiteInfo site)
		{
			this.Site = site;
			Crawlers = SitemapXmlConfiguration.Current.GetCrawlersForSite(Site.Name);
		}

		public IEnumerable<ISitemapNode> InitiateCrawl()
		{
			var nodes = new List<ISitemapNode>();

			foreach (var type in Crawlers)
			{
				try
				{
					if (!(Activator.CreateInstance(type, new { Site }) is Crawler crawler))
					{
						throw new Exception($"Could not activate Crawler type \"{type.FullName}\"");
					}

					nodes.AddRange(crawler.GetNodes());
				}
				catch (Exception ex)
				{
					Log.Error($"Constellation.Foundation.SitemapXml CrawlerManager: Error crawling site \"{Site.Name}\" using crawler \"{type.FullName}\"", ex, this);
				}
			}

			return nodes;
		}



	}
}
