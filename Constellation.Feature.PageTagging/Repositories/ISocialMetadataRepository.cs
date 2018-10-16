using Constellation.Feature.PageTagging.Models;
using Sitecore.Data.Items;

namespace Constellation.Feature.PageTagging.Repositories
{
	public interface ISocialMetadataRepository
	{
		PageSocialMetadata GetMetadata(Item contextItem);
	}
}
