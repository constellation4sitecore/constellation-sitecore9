using System;
using Sitecore.Data.Items;

namespace Constellation.Foundation.SitemapXml.Nodes
{
	public class DefaultSitemapNode : ItemBasedSitemapNode
	{
		protected override ChangeFrequency WhatIsTheItemsChangeFrequency(Item item)
		{
			return ChangeFrequency.Monthly;
		}

		protected override bool IsItemAPage(Item item)
		{
			return DoesTheItemHavePresentation(item);
		}

		protected override bool ShouldTheItemBeIndexedBySearchEngines(Item item)
		{
			return DoesTheItemHavePresentation(item);
		}

		protected override decimal WhatIsTheItemsIndexingPriority(Item item)
		{
			return 0.5M;
		}

		protected override bool DoesTheItemHavePresentation(Item item)
		{
			if (item.Visualization.Layout != null)
			{
				return true;
			}

			return false;
		}

		protected override DateTime WhenWasTheItemLastModified(Item item)
		{
			return Item.Statistics.Updated;
		}
	}
}
