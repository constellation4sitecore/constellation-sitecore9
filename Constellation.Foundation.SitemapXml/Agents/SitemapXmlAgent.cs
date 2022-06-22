using Constellation.Foundation.SitemapXml.Repositories;
using Sitecore.Diagnostics;
using Sitecore.Sites;

namespace Constellation.Foundation.SitemapXml.Agents
{
	/// <summary>
	/// This is an Agent that should be scheduled to run at regular intervals to generate the sitemap.xml
	/// files for all sites on the installation, and cache them for fast retrieval by actual requesting
	/// search engines.
	/// </summary>
	public class SitemapXmlAgent
	{
		/// <summary>
		/// This is the method the Scheduler should call when executing this agent.
		/// </summary>
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
				SitemapRepository.GetSitemap(new SiteContext(site), "sitemap.xml", false); // we want to catch when the cache is expired, no need to do work that's not needed.
				Log.Info($"Constellation.Foundation.SitemapXml {site.Name} sitemap.xml generated.", this);
			}
		}
	}
}
