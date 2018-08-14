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

			if (string.IsNullOrEmpty(Site.Scheme))
			{
				Log.Warn(
					$"Constellation.Foundation.SitemapXml CrawlerManager: Will not run crawlers for site {Site.Name} because it does not declare a \"scheme\" attribute.",
					this);
				return nodes;
			}

			if (string.IsNullOrEmpty(Site.TargetHostName))
			{
				Log.Warn(
					$"Constellation.Foundation.SitemapXml CrawlerManager: Will not run crawlers for site {Site.Name} because it does not declare a \"targetHostName\" attribute.",
					this);
				return nodes;
			}

			foreach (var type in Crawlers)
			{
				try
				{
					Crawler crawler = Activator.CreateInstance(type, new object[] { this.Site }) as Crawler;

					nodes.AddRange(crawler.GetNodes());
				}
				catch (Exception ex)
				{
					Log.Error($"Constellation.Foundation.SitemapXml CrawlerManager: Error crawling site \"{Site.Name}\" using crawler \"{type.FullName}\"", ex, this);
					throw;
				}
			}

			return nodes;
		}



	}
}
