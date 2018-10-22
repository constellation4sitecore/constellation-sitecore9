

using System;
using Constellation.Foundation.SitemapXml.Repositories;
using Sitecore.Diagnostics;

namespace Constellation.Foundation.SitemapXml.EventHandlers
{
	/// <summary>
	/// A Sitecore Event Handler that will generate sitemap.xml documents for all sites on the install
	/// (unless explicitly configured otherwise). Note that this event handler should only be attached to
	/// remote publish end events as: 1. It doesn't need to run on a CM server. 2. It will "hold" the publishing dialogs
	/// open until all sitemap.xml files are generated, which can take considerable time.
	/// </summary>
	public class RegenerateSitemapXml
	{
		/// <summary>
		/// The method called by the Event pipeline in Sitecore.
		/// </summary>
		/// <param name="sender">The sender</param>
		/// <param name="args">The event args</param>
		public void OnPublishEnd(object sender, EventArgs args)
		{
			// Get all the sites in the system
			var sites = Sitecore.Configuration.Factory.GetSiteInfoList();

			Log.Info($"Constellation.Foundation.SitemapXml OnPublishEnd regenerating {sites.Count} sitemap.xml documents", this);

			foreach (var site in sites)
			{
				if (SitemapXmlConfiguration.Current.SitesToIgnore.Contains(site.Name))
				{
					Log.Debug($"Constellation.Foundation.SitemapXml OnPublishEnd ignoring {site.Name} site.", this);
					continue;
				}

				Log.Info($"Constellation.Foundation.SitemapXml generating {site.Name} sitemap.xml", this);
				SitemapRepository.GetSitemap(site, true);
				Log.Info($"Constellation.Foundation.SitemapXml {site.Name} sitemap.xml generated.", this);
			}
		}
	}
}
