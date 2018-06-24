

using System;
using Constellation.Foundation.SitemapXml.Repositories;
using Sitecore.Diagnostics;

namespace Constellation.Foundation.SitemapXml.EventHandlers
{
	public class RegenerateSitemapXml
	{
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
