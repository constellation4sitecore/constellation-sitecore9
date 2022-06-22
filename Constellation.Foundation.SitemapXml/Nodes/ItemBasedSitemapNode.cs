using Constellation.Foundation.Globalization;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Web;
using System;
using System.Collections.Generic;

namespace Constellation.Foundation.SitemapXml.Nodes
{

	/// <summary>
	/// An implementation of ISitemapNode that inspects Sitecore Item attributes to
	/// populate its properties. This is the most common type of ISitemapNode implementation.
	/// </summary>
	public abstract class ItemBasedSitemapNode : ISitemapNode
	{
		#region Locals
		/// <summary>
		/// Gets the Item to interrogate.
		/// </summary>
		protected Item Item { get; private set; }


		/// <summary>
		/// Gets the Site that will host this node in a sitemap.xml document. Used to generate absolute URLs.
		/// </summary>
		protected SiteInfo Site { get; private set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Creates a new ItemBasedSitemapNode instance.
		/// </summary>
		protected ItemBasedSitemapNode()
		{

		}
		#endregion

		#region Properties

		/// <inheritdoc />
		public ChangeFrequency ChangeFrequency { get; private set; }

		/// <inheritdoc />
		public bool IsPage { get; private set; }

		/// <inheritdoc />
		public string Location { get; private set; }

		/// <inheritdoc />
		public decimal Priority { get; private set; }

		/// <inheritdoc />
		public bool ShouldIndex { get; private set; }

		/// <inheritdoc />
		public bool HasPresentation { get; private set; }

		/// <inheritdoc />
		public DateTime LastModified { get; private set; }

		/// <inheritdoc />
		public ICollection<AlternateLanguage> AlternateLanguages { get; private set; }

		/// <summary>
		/// Specifies whether to process alternate language variants of the given Item.
		/// </summary>
		protected bool IncludeAlternateLanguages { get; private set; }
		#endregion

		#region Methods

		/// <summary>
		/// Creates and initializes a new instance of an ItemBasedSitemapNode derived type.
		/// </summary>
		/// <param name="site">The Site whose sitemap.xml is being generated.</param>
		/// <param name="item">The Item to interrogate</param>
		/// <param name="includeAlternateLanguages">Whether to process alternate language variants of the given Item</param>
		/// <typeparam name="T">A derivative Type of ItemBasedSitemapNode</typeparam>
		/// <returns></returns>
		public static ItemBasedSitemapNode Create<T>(SiteInfo site, Item item, bool includeAlternateLanguages = false)
		where T : ItemBasedSitemapNode, new()
		{
			var output = new T
			{
				Site = site,
				Item = item,
				IncludeAlternateLanguages = includeAlternateLanguages
			};

			output.Initialize();

			return output;
		}

		/// <inheritdoc />
		public virtual bool IsValidForInclusionInSitemapXml()
		{
			if (!ShouldIndex)
			{
				return false;
			}

			if (!HasPresentation)
			{
				return false;
			}

			if (!IsPage)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Inspects the provided Item and Site and fills in the ISitemapNode properties.
		/// </summary>
		protected void Initialize()
		{
			AlternateLanguages = new List<AlternateLanguage>();
			HasPresentation = DoesTheItemHavePresentation(Item);
			IsPage = IsItemAPage(Item);
			ShouldIndex = ShouldTheItemBeIndexedBySearchEngines(Item);

			if (!HasPresentation || !IsPage || !ShouldIndex) return;


			Location = GetLocationUrl(Item, Site);
			Priority = WhatIsTheItemsIndexingPriority(Item);
			ChangeFrequency = WhatIsTheItemsChangeFrequency(Item);
			LastModified = WhenWasTheItemLastModified(Item);

			if (IncludeAlternateLanguages)
			{
				GetAlternateLanguages(Item, Site);
			}
		}



		/// <summary>
		/// Use this method to provide logic for generating the Change Frequency.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected abstract ChangeFrequency WhatIsTheItemsChangeFrequency(Item item);

		/// <summary>
		/// Use this method to provide logic for determining if the supplied Item is a page.
		/// </summary>
		/// <param name="item">The Item to inspect.</param>
		/// <returns>True if the Item should be included in the sitemap.xml</returns>
		protected abstract bool IsItemAPage(Item item);

		/// <summary>
		/// Use this method to provide logic for determining if the supplied Item should be indexed by search engines.
		/// </summary>
		/// <param name="item">The Item to inspect</param>
		/// <returns>True if the item should be included in the sitemap.xml</returns>
		protected abstract bool ShouldTheItemBeIndexedBySearchEngines(Item item);

		/// <summary>
		/// Use this method to provide logic for determining the Indexing Priority for the supplied Item.
		/// </summary>
		/// <param name="item">The Item to inspect.</param>
		/// <returns>The indexing priority (0 to 1) of the item when included in the sitemap.xml</returns>
		protected abstract decimal WhatIsTheItemsIndexingPriority(Item item);

		/// <summary>
		/// Use this method to provide logic for determining if the supplied Item has presentation details.
		/// </summary>
		/// <param name="item">The item to inspect.</param>
		/// <returns>True if the item should be included in the sitemap.xml</returns>
		protected abstract bool DoesTheItemHavePresentation(Item item);

		/// <summary>
		/// Use this method to provide logic for determining the last modified time for the supplied Item.
		/// </summary>
		/// <param name="item">The item to inspect.</param>
		/// <returns>A DateTime representing the value that should be written to the sitemap.xml document.</returns>
		protected abstract DateTime WhenWasTheItemLastModified(Item item);


		#endregion

		#region Helpers
		/// <summary>
		/// Gets the Item's absolute Url.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="site">The site to use for hostname, scheme, and custom linkProvider parameters.</param>
		/// <returns></returns>
		protected virtual string GetLocationUrl(Item item, SiteInfo site)
		{
			return NodeLinkManager.GetNodeLocationUrl(item, site);
		}

		/// <summary>
		/// For the given Site and Item, adds an AlternateLanguage object to the AlternateLanguages collection for each valid SupportedLanguage.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="site"></param>
		protected virtual void GetAlternateLanguages(Item item, SiteInfo site)
		{
			var languageCodes = site.SupportedLanguages();

			foreach (var code in languageCodes)
			{
				if (!Language.TryParse(code, out var language)) continue;

				var altItem = item.Database.GetItem(item.ID, language);

				if (altItem == null || altItem.Empty) continue;

				var alternateLanguage = new AlternateLanguage
				{
					Href = NodeLinkManager.GetNodeLocationUrl(altItem, site),
					HrefLang = code
				};
				AlternateLanguages.Add(alternateLanguage);
			}
		}
		#endregion
	}
}
