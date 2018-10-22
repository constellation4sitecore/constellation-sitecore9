using Constellation.Feature.PageTagging.Models;
using Sitecore.Data.Items;

namespace Constellation.Feature.PageTagging.Repositories
{
	/// <summary>
	/// A repository capable of parsing an Item to create a PageMetadata model. The default implementation
	/// of this contract includes logic to visit Item parents to find valid values for Meta Author and Meta Publisher.
	/// </summary>
	public interface IMetadataRepository
	{
		/// <summary>
		/// Converts appropriate fields on the context Item to values on the PageMetadata model.
		/// </summary>
		/// <param name="contextItem">The item to inspect.</param>
		/// <returns>a populated PageMetadata model.</returns>
		PageMetadata GetMetadata(Item contextItem);

		/// <summary>
		/// Assuming the Sitecore installation makes use of the Page Metadata template, reviews the provided Item
		/// for Meta Author and Meta Publisher values. If they are not found, there is a checkbox indicating that
		/// they can be inherited, at which point the Repository will call ancestor Items sequentially until it finds
		/// valid values, or it exits the top of the site.
		/// </summary>
		/// <param name="context">The Item to inspect.</param>
		/// <param name="model">The model to fill with Author and Publisher values.</param>
		void FillAuthorAndPublisher(Item context, PageMetadata model);
	}
}
