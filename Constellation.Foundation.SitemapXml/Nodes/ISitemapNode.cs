using System;
using System.Collections.Generic;

namespace Constellation.Foundation.SitemapXml.Nodes
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
		/// Gets a value indicating whether the Item is a page.
		/// </summary>
		bool IsPage { get; }

		/// <summary>
		/// This is the absolute URL for the target Item.
		/// </summary>
		string Location { get; }

		/// <summary>
		/// Gets the indexing priority of this node on a scale from 0.0 to 1.0.
		/// 0.0 is lowest priority.
		/// 1.0 is highest priority.
		/// neutral is 0.5.
		/// </summary>
		decimal Priority { get; }

		/// <summary>
		/// Gets a value indicating whether the search crawler should index the Item.
		/// </summary>
		bool ShouldIndex { get; }

		/// <summary>
		/// Gets a value indicating whether the Item has presentation details configured.
		/// </summary>
		bool HasPresentation { get; }

		/// <summary>
		/// A collection of Alternate Language links that need to be included in the Sitemap file.
		/// </summary>
		ICollection<AlternateLanguage> AlternateLanguages { get; }


		/// <summary>
		/// Gets the last modified date of the Item.
		/// </summary>
		DateTime LastModified { get; }

		/// <summary>
		/// Definitive test for inclusion in Sitemap happens here, preferably by evaluating
		/// the other boolean properties available.
		/// </summary>
		bool IsValidForInclusionInSitemapXml();
	}
}
