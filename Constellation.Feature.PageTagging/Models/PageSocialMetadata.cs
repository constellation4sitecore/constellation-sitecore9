using Constellation.Foundation.ModelMapping.MappingAttributes;

namespace Constellation.Feature.PageTagging.Models
{
	public class PageSocialMetadata : PageMetadata
	{
		[RenderAsUrl()]
		public string SocialThumbnail { get; set; }

		public TwitterCardType TwitterCardTypeTargetItem { get; set; }

		[RawValueOnly]
		public string TwitterSite { get; set; }

		[RawValueOnly]
		public string TwitterCreator { get; set; }

		public bool InheritTwitterValues { get; set; }
	}
}
