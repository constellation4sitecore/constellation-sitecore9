using Constellation.Feature.PageTagging.SitemapXml.Models;
using Constellation.Foundation.ModelMapping;
using Constellation.Foundation.SitemapXml.Nodes;
using Sitecore.Data.Items;
using ChangeFrequency = Constellation.Foundation.SitemapXml.ChangeFrequency;

namespace Constellation.Feature.PageTagging.SitemapXml
{
	public class SitemapNode : DefaultSitemapNode
	{
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

		protected override ChangeFrequency WhatIsTheItemsChangeFrequency(Item item)
		{
			var model = item.MapToNew<PageSitemapBehavior>();

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
