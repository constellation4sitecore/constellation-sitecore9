using Constellation.Feature.PageTagging.Models;
using Sitecore.Data.Items;

namespace Constellation.Feature.PageTagging.Repositories
{
	/// <summary>
	/// A repository that is capable of reading an Item implementing the Page Social Metadata template and
	/// converting appropriate values to a PageSocialMetadata model.
	/// </summary>
	public interface ISocialMetadataRepository
	{
		/// <summary>
		/// Inspects the provided Item and populates values on a new PageSocialMetadata model instance.
		/// Note that some of the values are allowed to be inherited from Parent Items, in which case the
		/// repository will inspect Ancestors until it comes to valid values or it exits the top of the site.
		/// </summary>
		/// <param name="contextItem">The Item to inspect.</param>
		/// <returns>A populated PageSocialMetadata model.</returns>
		PageSocialMetadata GetMetadata(Item contextItem);
	}
}
