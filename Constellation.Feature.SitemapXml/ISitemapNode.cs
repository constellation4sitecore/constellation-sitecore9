using System;

namespace Constellation.Feature.SitemapXml
{
	/// <summary>
	/// The base contract for an object that represents a node in a sitemap.xml document.
	/// The node object is used to evaluate a Sitecore Item.
	/// </summary>
	public interface ISitemapNode
	{
		/// <summary>
		/// Gets the change frequency.
		/// </summary>
		ChangeFrequency ChangeFrequency { get; }

		/// <summary>
		/// Gets a value indicating whether the Item is listed in site navigation.
		/// </summary>
		bool IsListedInNavigation { get; }

		/// <summary>
		/// Gets a value indicating whether the Item is a page.
		/// </summary>
		bool IsPage { get; }

		/// <summary>
		/// Gets the absolute URL for the Item.
		/// </summary>
		string Location { get; }

		/// <summary>
		/// Gets the search crawler indexing priority for the Item, values are 0.0 to 1.0.
		/// </summary>
		decimal Priority { get; }

		/// <summary>
		/// Gets a value indicating whether the search crawler should index the Item.
		/// </summary>
		bool ShouldIndex { get; }

		/// <summary>
		/// Gets the last modified date of the Item.
		/// </summary>
		DateTime LastModified { get; }
	}
}
