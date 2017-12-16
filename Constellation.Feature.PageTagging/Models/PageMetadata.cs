using Constellation.Foundation.ModelMapping.MappingAttributes;

namespace Constellation.Feature.PageTagging.Models
{
	public class PageMetadata
	{
		[RawValueOnly]
		public string BrowserTitle { get; set; }

		[RawValueOnly]
		public string Keywords { get; set; }

		[RawValueOnly]
		public string MetaDescription { get; set; }

		[RawValueOnly]
		public string MetaAuthor { get; set; }

		[RawValueOnly]
		public string MetaPublisher { get; set; }

		public bool InheritAuthorAndPublisher { get; set; }

		public bool HasValidAuthorAndPublisher => !InheritAuthorAndPublisher || !string.IsNullOrEmpty(MetaAuthor) && !string.IsNullOrEmpty(MetaPublisher);

		public bool HasValidAuthor => !InheritAuthorAndPublisher || !string.IsNullOrEmpty(MetaAuthor);

		public bool HasValidPublisher => !InheritAuthorAndPublisher || !string.IsNullOrEmpty(MetaPublisher);
	}
}
