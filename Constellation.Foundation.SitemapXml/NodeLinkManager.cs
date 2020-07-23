using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Links;
using Sitecore.Links.UrlBuilders;
using Sitecore.Sites;
using Sitecore.Web;

namespace Constellation.Foundation.SitemapXml
{
	/// <summary>
	/// Handles the background-thread retrieval of a valid LinkProvider and ensures that generated Item links consider:
	/// - The Site provided.
	/// - Use the correct site-specific LinkProvider.
	/// </summary>
	public static class NodeLinkManager
	{
		/// <summary>
		/// Gets the ProviderHelper from the Service Locator.
		/// </summary>
		public static ProviderHelper<LinkProvider, LinkProviderCollection> ProviderHelper
		{
			get { return (ProviderHelper<LinkProvider, LinkProviderCollection>)ServiceLocator.ServiceProvider.GetService(typeof(ProviderHelper<LinkProvider, LinkProviderCollection>)); }
		}

		/// <summary>
		/// Gets an absolute URL to the Item using the Site provided and if available, the Site-specific Link Provider.
		/// </summary>
		/// <param name="item">The Item to use to generate a URL</param>
		/// <param name="site">The Site to use for generating the hostname and setting any URL Options.</param>
		/// <returns>an absolute URL string.</returns>
		public static string GetNodeLocationUrl(Item item, SiteInfo site)
		{
			var siteContext = new SiteContext(site);
			var provider = GetSiteProvider(siteContext);

			var options = provider.GetDefaultUrlBuilderOptions();

            Sitecore.Context.Site = siteContext;
            options.AlwaysIncludeServerUrl = true;
			
            return provider.GetItemUrl(item, options);
		}

		/// <summary>
		/// Give a Site Context attempts to resolve the linkProvider to a named configuration of Provider.
		/// </summary>
		/// <param name="site">The Site to parse.</param>
		/// <returns>The LinkProvider to use.</returns>
		private static LinkProvider GetSiteProvider(SiteContext site)
		{
			var siteLinkProvider = GetSiteProviderName(site);

			if (string.IsNullOrEmpty(siteLinkProvider))
			{
				return ProviderHelper.Provider;
			}

			return ProviderHelper.Providers[siteLinkProvider]
				   ?? ProviderHelper.Provider;
		}

		/// <summary>
		/// Given a Site Context attempts to resolve the linkProvider property to a string name.
		/// </summary>
		/// <param name="site">The Site to parse.</param>
		/// <returns>The name of the LinkProvider configuration to use.</returns>
		private static string GetSiteProviderName(SiteContext site)
		{
			var setting = site?.Properties?["linkProvider"];

			return setting;
		}
	}
}
