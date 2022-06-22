using System;
using System.Collections.Generic;
using Constellation.Foundation.SitemapXml.Crawlers;
using Constellation.Foundation.SitemapXml.Nodes;
using Sitecore.Diagnostics;
using Sitecore.Web;

namespace Constellation.Foundation.SitemapXml
{
	/// <summary>
	/// Reads the active configuration and loads the correct crawlers for the supplied Site.
	/// </summary>
	public class CrawlerManager
	{

		/// <summary>
		/// The Site to crawl.
		/// </summary>
		public SiteInfo Site { get; private set; }

		/// <summary>
		/// The crawlers that will be used to crawl the site.
		/// </summary>
		public ICollection<Type> Crawlers { get; private set; }


		/// <summary>
		/// Creates a new instance of CrawlerManager and specifies the Site that is to be crawled.
		/// </summary>
		/// <param name="site"></param>
		public CrawlerManager(SiteInfo site)
		{
			this.Site = site;
			Crawlers = SitemapXmlConfiguration.Current.GetCrawlersForSite(Site.Name);
		}

		/// <summary>
		/// Loads all Crawlers for the Site and runs them, producing an output of Sitemap Nodes
		/// that can be added to the sitemap.xml document.
		/// </summary>
		/// <param name="IncludeLanguageVariants">True if Nodes should include information about alternate language URLs</param>
		/// <returns>An enumerable of ISitemapNode. Note that these nodes should be inspected for suitability before adding them to the sitemap.xml document.</returns>
		public IEnumerable<ISitemapNode> InitiateCrawl(bool IncludeLanguageVariants = false)
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

					// ReSharper disable once PossibleNullReferenceException
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
