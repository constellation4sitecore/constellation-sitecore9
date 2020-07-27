using Sitecore.Abstractions;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Links.UrlBuilders;
using Sitecore.Sites;
using Sitecore.Web;
using System;
using System.Web;

namespace Constellation.Foundation.Linking
{
	/// <summary>
	/// Allows the user to provide a custom LinkProvider on a site-by-site basis, which includes changing the behavior of generated links.
	/// </summary>
	/// <remarks>
	/// Taken with permission from https://jammykam.wordpress.com/2017/09/22/switching-link-manager/
	/// Original code: https://gist.github.com/jammykam/daa151abeeac8a7029bf
	/// </remarks>
	public class SwitchingLinkManager : BaseLinkManager
	{
		private readonly ProviderHelper<LinkProvider, LinkProviderCollection> _providerHelper;
		private readonly IItemSiteResolver _itemSiteResolver;

		/// <summary>
		/// Creates a new instance of SwitchingLinkManager
		/// </summary>
		/// <param name="providerHelper">The providerHelper</param>
		/// <param name="itemSiteResolver">The itemSiteResolver</param>
		public SwitchingLinkManager(ProviderHelper<LinkProvider, LinkProviderCollection> providerHelper, IItemSiteResolver itemSiteResolver)
		{
			_providerHelper = providerHelper;
			_itemSiteResolver = itemSiteResolver;
		}

		/// <summary>
		/// The site-specific LinkProvider if there is a context site and the site has a provider, else the default provider.
		/// </summary>
		protected virtual LinkProvider Provider
		{
			get { return GetSiteProvider(Sitecore.Context.Site); }
		}

		/* below is copy/paste from Sitecore.Links.DefaultLinkManager because Provider is marked internal :'( */

		/// <inheritdoc />
		public override bool AddAspxExtension => Provider.AddAspxExtension;

		/// <inheritdoc />
		public override bool AlwaysIncludeServerUrl => Provider.AlwaysIncludeServerUrl;

		/// <inheritdoc />
		public override LanguageEmbedding LanguageEmbedding => Provider.LanguageEmbedding;

		/// <inheritdoc />
		public override LanguageLocation LanguageLocation => Provider.LanguageLocation;

		/// <inheritdoc />
		public override bool LowercaseUrls => Provider.LowercaseUrls;

		/// <inheritdoc />
		public override bool ShortenUrls => Provider.ShortenUrls;

		/// <inheritdoc />
		public override bool UseDisplayName => Provider.UseDisplayName;

		/// <inheritdoc />
		public override string ExpandDynamicLinks(string text)
		{
			Assert.ArgumentNotNull(text, nameof(text));
			return ExpandDynamicLinks(text, false);
		}

		/// <inheritdoc />
		public override string ExpandDynamicLinks(string text, bool resolveSites)
		{
			Assert.ArgumentNotNull(text, nameof(text));
			return Assert.ResultNotNull(Provider.ExpandDynamicLinks(text, resolveSites));
		}

		/// <inheritdoc />
		[Obsolete]
		public override UrlOptions GetDefaultUrlOptions()
		{
			return Assert.ResultNotNull(Provider.GetDefaultUrlOptions());
		}

		/// <inheritdoc />
		public override ItemUrlBuilderOptions GetDefaultUrlBuilderOptions()
		{
			return Assert.ResultNotNull(Provider.GetDefaultUrlBuilderOptions());
		}

		/// <inheritdoc />
		public override string GetDynamicUrl(Item item)
		{
			return GetDynamicUrl(item, LinkUrlOptions.Empty);
		}

		/// <inheritdoc />
		public override string GetDynamicUrl(Item item, LinkUrlOptions options)
		{
			return Provider.GetDynamicUrl(item, options);
		}

		/// <inheritdoc />
		public override string GetItemUrl(Item item)
		{
			return Provider.GetItemUrl(item, GetDefaultUrlBuilderOptions());
		}

		/// <inheritdoc />
		public override string GetItemUrl(Item item, ItemUrlBuilderOptions options)
		{
			var provider = GetSiteProvider(options.Site);

			return provider.GetItemUrl(item, options);
		}

		/// <inheritdoc />
		public override bool IsDynamicLink(string linkText)
		{
			return Provider.IsDynamicLink(linkText);
		}

		/// <inheritdoc />
		public override DynamicLink ParseDynamicLink(string linkText)
		{
			return Provider.ParseDynamicLink(linkText);
		}

		/// <inheritdoc />
		public override RequestUrl ParseRequestUrl(HttpRequest request)
		{
			return Provider.ParseRequestUrl(new HttpRequestWrapper(request));
		}

		/// <inheritdoc />
		//TODO: 9.3 check IItemSiteResolver Injection
		[Obsolete("Please use IItemSiteResolver instead.")]
		public override SiteInfo ResolveTargetSite(Item item)
		{
			Assert.ArgumentNotNull(item, nameof(item));
			return _itemSiteResolver.ResolveSite(item);
		}

		/// <inheritdoc />
		public override SiteContext GetPreviewSiteContext(Item item)
		{
			Assert.ArgumentNotNull(item, nameof(item));
			return Provider.GetPreviewSiteContext(item);
		}


		/// <summary>
		/// Give a Site Context attempts to resolve the linkProvider to a named configuration of Provider.
		/// </summary>
		/// <param name="site">The Site to parse.</param>
		/// <returns>The LinkProvider to use.</returns>
		protected LinkProvider GetSiteProvider(SiteContext site)
		{
			var siteLinkProvider = GetSiteProviderName(site);

			if (string.IsNullOrEmpty(siteLinkProvider))
				return _providerHelper.Provider;

			return _providerHelper.Providers[siteLinkProvider]
				   ?? _providerHelper.Provider;
		}

		/// <summary>
		/// Given a Site Context attempts to resolve the linkProvider property to a string name.
		/// </summary>
		/// <param name="site">The Site to parse.</param>
		/// <returns>The name of the LinkProvider configuration to use.</returns>
		protected string GetSiteProviderName(SiteContext site)
		{
			var setting = site?.Properties?["linkProvider"];

			return setting;
		}
	}
}
