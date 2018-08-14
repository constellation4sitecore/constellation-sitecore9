using Constellation.Foundation.SitemapXml.Repositories;
using Sitecore.Diagnostics;

namespace Constellation.Foundation.SitemapXml.Agents
{
	public class SitemapXmlAgent
	{
		public void Refresh()
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
				SitemapRepository.GetSitemap(site, false); // we want to catch when the cache is expired, no need to do work that's not needed.
				Log.Info($"Constellation.Foundation.SitemapXml {site.Name} sitemap.xml generated.", this);
			}
		}
	}
}
