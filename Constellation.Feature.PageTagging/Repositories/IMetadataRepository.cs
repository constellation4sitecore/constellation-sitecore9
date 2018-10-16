using Constellation.Feature.PageTagging.Models;
using Sitecore.Data.Items;

namespace Constellation.Feature.PageTagging.Repositories
{
	public interface IMetadataRepository
	{
		PageMetadata GetMetadata(Item contextItem);
		void FillAuthorAndPublisher(Item context, PageMetadata model);
	}
}
