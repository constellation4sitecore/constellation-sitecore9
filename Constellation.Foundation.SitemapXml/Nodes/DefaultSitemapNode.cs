using System;
using Sitecore.Data.Items;

namespace Constellation.Foundation.SitemapXml.Nodes
{
	/// <summary>
	/// A safe implementation of ItemBasedSitemapNode that will produce a usable sitemap.xml document
	/// on most Sitecore sites.
	/// </summary>
	public class DefaultSitemapNode : ItemBasedSitemapNode
	{
		/// <summary>
		/// Always returns Monthly
		/// </summary>
		/// <param name="item">The item to inspect</param>
		/// <returns>Monthly</returns>
		protected override ChangeFrequency WhatIsTheItemsChangeFrequency(Item item)
		{
			return ChangeFrequency.Monthly;
		}

		/// <summary>
		/// True if the Item has presentation.
		/// </summary>
		/// <param name="item">The itme to inspect</param>
		/// <returns>the value of DoesTheItemHavePresentation()</returns>
		protected override bool IsItemAPage(Item item)
		{
			return DoesTheItemHavePresentation(item);
		}

		/// <summary>
		/// True if the Item has presentation.
		/// </summary>
		/// <param name="item">The itme to inspect</param>
		/// <returns>the value of DoesTheItemHavePresentation()</returns>
		protected override bool ShouldTheItemBeIndexedBySearchEngines(Item item)
		{
			return DoesTheItemHavePresentation(item);
		}

		/// <summary>
		/// Always returns the "safe" default of 0.5
		/// </summary>
		/// <param name="item">The Item to inspect</param>
		/// <returns>0.5 (medium priority)</returns>
		protected override decimal WhatIsTheItemsIndexingPriority(Item item)
		{
			return 0.5M;
		}

		/// <summary>
		/// Determine if the Item has configured presentation details.
		/// </summary>
		/// <param name="item">The Item to inspect.</param>
		/// <returns>True if the Item has a Layout set.</returns>
		protected override bool DoesTheItemHavePresentation(Item item)
		{
			if (item.Visualization.Layout != null)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Returns the Item's Statistics.Updated value.
		/// </summary>
		/// <param name="item">The Item</param>
		/// <returns>Item.Statistics.Updated</returns>
		protected override DateTime WhenWasTheItemLastModified(Item item)
		{
			return Item.Statistics.Updated;
		}
	}
}
