using Constellation.Feature.PageTagging.SitemapXml.Models;
using Constellation.Foundation.ModelMapping;
using Constellation.Foundation.SitemapXml.Nodes;
using Sitecore.Data.Items;
using ChangeFrequency = Constellation.Foundation.SitemapXml.ChangeFrequency;

namespace Constellation.Feature.PageTagging.SitemapXml
{
	/// <summary>
	/// A sitemap.xml Node that takes advantage of the Page Search Engine Directives template to determine
	/// if a page should be listed in the sitemap.xml
	/// </summary>
	public class SitemapNode : DefaultSitemapNode
	{
		/// <summary>
		/// Uses a combination of IncludeInSitemap, SearchEngineIndexesPage and SearchEngineFollowsLinks to
		/// determine if the item should be indexed.
		/// </summary>
		/// <param name="item">The Item to inspect.</param>
		/// <returns>True if IncludeInSitemap is true and SearchEngineIndexesPage is true or SearchEngineFollowsLinks is true.</returns>
		protected override bool ShouldTheItemBeIndexedBySearchEngines(Item item)
		{
			var model = item.MapToNew<PageSitemapBehavior>();

			if (!model.IncludeInSitemap)
			{
				return false;
			}

			if (!model.SearchEngineIndexesPage && !model.SearchEngineFollowsLinks)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Uses the Change Frequency field to determine the change frequency to return.
		/// </summary>
		/// <param name="item">The Item to inspect.</param>
		/// <returns>The change frequency.</returns>
		protected override ChangeFrequency WhatIsTheItemsChangeFrequency(Item item)
		{
			var model = item.MapToNew<PageSitemapBehavior>();

			if (model.ChangeFrequency == null)
			{
				return ChangeFrequency.Monthly;
			}

			switch (model.ChangeFrequency.DisplayName)
			{
				case "always":
					return ChangeFrequency.Always;
				case "hourly":
					return ChangeFrequency.Hourly;
				case "daily":
					return ChangeFrequency.Daily;
				case "weekly":
					return ChangeFrequency.Weekly;
				case "yearly":
					return ChangeFrequency.Yearly;
				case "never":
					return ChangeFrequency.Never;
				default:
					return ChangeFrequency.Monthly;
			}
		}
	}
}
