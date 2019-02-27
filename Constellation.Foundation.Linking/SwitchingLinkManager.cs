using System;
using System.Web;
using Sitecore.Abstractions;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Sites;
using Sitecore.Web;

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

		/// <summary>
		/// Creates a new instance of SwitchingLinkManager
		/// </summary>
		/// <param name="providerHelper">The providerHelper</param>
		public SwitchingLinkManager(ProviderHelper<LinkProvider, LinkProviderCollection> providerHelper)
		{
			_providerHelper = providerHelper;
		}

		/// <summary>
		/// The site-specific LinkProvider if there is a context site and the site has a provider, else the default provider.
		/// </summary>
		protected virtual LinkProvider Provider
		{
			get
			{
				var siteLinkProvider = Sitecore.Context.Site?.Properties?["linkProvider"];

				if (String.IsNullOrEmpty(siteLinkProvider))
					return _providerHelper.Provider;

				return _providerHelper.Providers[siteLinkProvider]
					   ?? _providerHelper.Provider;
			}
		}

		/* below is copy/paste from Sitecore.Links.DefaultLinkManager cos Provider is marked internal :'( */

		/// <inheritdoc />
		public override bool AddAspxExtension => this.Provider.AddAspxExtension;

		/// <inheritdoc />
		public override bool AlwaysIncludeServerUrl => this.Provider.AlwaysIncludeServerUrl;

		/// <inheritdoc />
		public override LanguageEmbedding LanguageEmbedding => this.Provider.LanguageEmbedding;

		/// <inheritdoc />
		public override LanguageLocation LanguageLocation => this.Provider.LanguageLocation;

		/// <inheritdoc />
		public override bool LowercaseUrls => this.Provider.LowercaseUrls;

		/// <inheritdoc />
		public override bool ShortenUrls => this.Provider.ShortenUrls;

		/// <inheritdoc />
		public override bool UseDisplayName => this.Provider.UseDisplayName;

		/// <inheritdoc />
		public override string ExpandDynamicLinks(string text)
		{
			Assert.ArgumentNotNull(text, nameof(text));
			return this.ExpandDynamicLinks(text, false);
		}

		/// <inheritdoc />
		public override string ExpandDynamicLinks(string text, bool resolveSites)
		{
			Assert.ArgumentNotNull(text, nameof(text));
			return Assert.ResultNotNull<string>(this.Provider.ExpandDynamicLinks(text, resolveSites));
		}

		/// <inheritdoc />
		public override UrlOptions GetDefaultUrlOptions()
		{
			return Assert.ResultNotNull<UrlOptions>(this.Provider.GetDefaultUrlOptions());
		}

		/// <inheritdoc />
		public override string GetDynamicUrl(Item item)
		{
			return this.GetDynamicUrl(item, LinkUrlOptions.Empty);
		}

		/// <inheritdoc />
		public override string GetDynamicUrl(Item item, LinkUrlOptions options)
		{
			return this.Provider.GetDynamicUrl(item, options);
		}

		/// <inheritdoc />
		public override string GetItemUrl(Item item)
		{
			return this.Provider.GetItemUrl(item, this.GetDefaultUrlOptions());
		}

		/// <inheritdoc />
		public override string GetItemUrl(Item item, UrlOptions options)
		{
			return this.Provider.GetItemUrl(item, options);
		}

		/// <inheritdoc />
		public override bool IsDynamicLink(string linkText)
		{
			return this.Provider.IsDynamicLink(linkText);
		}

		/// <inheritdoc />
		public override DynamicLink ParseDynamicLink(string linkText)
		{
			return this.Provider.ParseDynamicLink(linkText);
		}

		/// <inheritdoc />
		public override RequestUrl ParseRequestUrl(HttpRequest request)
		{
			return this.Provider.ParseRequestUrl(request);
		}

		/// <inheritdoc />
		public override SiteInfo ResolveTargetSite(Item item)
		{
			Assert.ArgumentNotNull(item, nameof(item));
			return this.Provider.ResolveTargetSite(item);
		}

		/// <inheritdoc />
		public override SiteContext GetPreviewSiteContext(Item item)
		{
			Assert.ArgumentNotNull(item, nameof(item));
			return this.Provider.GetPreviewSiteContext(item);
		}
	}
}
