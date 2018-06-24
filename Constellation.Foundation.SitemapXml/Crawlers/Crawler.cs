using System.Collections.Generic;
using Constellation.Foundation.SitemapXml.Nodes;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Web;

namespace Constellation.Foundation.SitemapXml.Crawlers
{
	public abstract class Crawler
	{
		#region properties
		protected Database Database { get; }

		protected Language Language { get; }

		protected SiteInfo Site { get; }


		#endregion

		#region Constructor
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

		public ICollection<ISitemapNode> GetNodes()
		{
			return GetNodes(Site, Database, Language);
		}

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
