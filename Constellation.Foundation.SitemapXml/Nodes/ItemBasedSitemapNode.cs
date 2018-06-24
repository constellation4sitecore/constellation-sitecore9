using System;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Sites;
using Sitecore.Web;

namespace Constellation.Foundation.SitemapXml.Nodes
{
	public abstract class ItemBasedSitemapNode : ISitemapNode
	{
		#region Locals
		protected Item Item { get; private set; }


		protected SiteInfo Site { get; private set; }

		#endregion

		#region Constructor

		protected ItemBasedSitemapNode()
		{

		}
		#endregion

		#region Properties
		public ChangeFrequency ChangeFrequency { get; private set; }
		public bool IsPage { get; private set; }
		public string Location { get; private set; }
		public decimal Priority { get; private set; }
		public bool ShouldIndex { get; private set; }
		public bool HasPresentation { get; private set; }
		public DateTime LastModified { get; private set; }
		#endregion

		#region Methods

		public static ItemBasedSitemapNode Create<T>(SiteInfo site, Item item)
		where T : ItemBasedSitemapNode, new()
		{
			var output = new T
			{
				Site = site,
				Item = item
			};

			output.Initialize();

			return output;
		}

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

		protected void Initialize()
		{
			ChangeFrequency = WhatIsTheItemsChangeFrequency(Item);
			IsPage = IsItemAPage(Item);
			Location = GetLocationUrl(Item);
			Priority = WhatIsTheItemsIndexingPriority(Item);
			ShouldIndex = ShouldTheItemBeIndexedBySearchEngines(Item);
			HasPresentation = DoesTheItemHavePresentation(Item);
			LastModified = WhenWasTheItemLastModified(Item);
		}

		protected abstract ChangeFrequency WhatIsTheItemsChangeFrequency(Item item);

		protected abstract bool IsItemAPage(Item item);

		protected abstract bool ShouldTheItemBeIndexedBySearchEngines(Item item);

		protected abstract decimal WhatIsTheItemsIndexingPriority(Item item);

		protected abstract bool DoesTheItemHavePresentation(Item item);

		protected abstract DateTime WhenWasTheItemLastModified(Item item);


		#endregion

		#region Helpers
		protected virtual string GetLocationUrl(Item item)
		{
			var options = LinkManager.GetDefaultUrlOptions();

			return GetLocationUrl(item, options.LanguageEmbedding, options.LanguageLocation);
		}

		protected virtual string GetLocationUrl(Item item, LanguageEmbedding languageEmbedding,
			LanguageLocation languageLocation)
		{
			return GetLocationUrl(item, Site, languageEmbedding, languageLocation);
		}

		protected virtual string GetLocationUrl(Item item, SiteInfo site, LanguageEmbedding languageEmbedding,
			LanguageLocation languageLocation)
		{
			var options = LinkManager.GetDefaultUrlOptions();

			options.LanguageEmbedding = languageEmbedding;
			options.LanguageLocation = languageLocation;
			options.Language = item.Language;
			options.Site = new SiteContext(site);
			options.AlwaysIncludeServerUrl = true;

			return LinkManager.GetItemUrl(item, options);
		}
		#endregion
	}
}
