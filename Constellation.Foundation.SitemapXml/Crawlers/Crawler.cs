using System.Collections.Generic;
using Constellation.Foundation.SitemapXml.Nodes;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Web;

namespace Constellation.Foundation.SitemapXml.Crawlers
{
	/// <summary>
	/// An object that will interrogate the Sitecore installation, and, within the context of a
	/// specific Site, Language, and Database, produce a list of Sitemap Nodes for the sitemap.xml
	/// document for the given Site.
	/// </summary>
	public abstract class Crawler
	{
		#region properties
		/// <summary>
		/// The database to query.
		/// </summary>
		protected Database Database { get; }

		/// <summary>
		/// The language to limit Item results to.
		/// </summary>
		protected Language Language { get; }

		/// <summary>
		/// The Site to limit Item results to.
		/// </summary>
		protected SiteInfo Site { get; }


		#endregion

		#region Constructor
		/// <summary>
		/// Creates a new instance of Crawler and sets the context Properties for the instance.
		/// </summary>
		/// <param name="site"></param>
		protected Crawler(SiteInfo site)
		{
			Site = site;

			Language = GetDefaultLanguage(site);

			Database = Sitecore.Configuration.Factory.GetDatabase(site.Database);

			Assert.IsNotNullOrEmpty(site.Scheme, $"Constellation.Foundation.SitemapXml: Error processing \"{site.Name}\". SiteInfo.Scheme cannot be empty.");
			Assert.IsNotNullOrEmpty(site.TargetHostName, $"Constellation.Foundation.SitemapXml: Error processing \"{site.Name}\". SiteInfo.TargetHostName cannot be empty.");
		}
		#endregion

		#region Methods

		/// <summary>
		/// Crawls the Sitecore installation within the Context Properties and finds matching Sitemap Nodes to return
		/// for the sitemap.xml document.
		/// </summary>
		/// <returns>A colleciton of SitemapNodes to be inspected for inclusion in the sitemap.xml document.</returns>
		public ICollection<ISitemapNode> GetNodes()
		{
			return GetNodes(Site, Database, Language);
		}

		/// <summary>
		/// Implement this method to crawl the Sitecore install given the context parameters of the method.
		/// </summary>
		/// <param name="site">The site to crawl.</param>
		/// <param name="database">The database to crawl.</param>
		/// <param name="language">Sitemap Nodes should only be generated if the source Items have matching Language versions.</param>
		/// <returns>A colleciton of Sitemap Nodes to be reviewed for inclusion on the sitemap.xml document.</returns>
		protected abstract ICollection<ISitemapNode> GetNodes(SiteInfo site, Database database, Language language);

		private Language GetDefaultLanguage(SiteInfo site)
		{
			var langCode = site.Language;

			if (string.IsNullOrEmpty(langCode))
			{
				Log.Warn($"Constellation.Foundation.SitemapXml Crawler: No default language set for site {Sitecore.Context.Site.Name}. Using Sitecore's defaultLanguage setting.", this);
				langCode = Sitecore.Configuration.Settings.DefaultLanguage;
			}

			if (!Language.TryParse(langCode, out Language language))
			{
				Log.Warn($"Constellation.Foundation.SitemapXml Crawler: Could not resolve language with code {langCode} for {site.Name} ", this);
			}

			return language;
		}
		#endregion
	}
}
