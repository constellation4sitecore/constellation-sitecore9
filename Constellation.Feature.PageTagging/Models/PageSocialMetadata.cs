using Constellation.Foundation.ModelMapping.MappingAttributes;

namespace Constellation.Feature.PageTagging.Models
{
	/// <summary>
	/// A ViewModel that represents the social meta tags that allow pages shared on twitter and facebook to have nice summary cards.
	/// </summary>
	public class PageSocialMetadata : PageMetadata
	{
		/// <summary>
		/// The image url for the og:image tag.
		/// </summary>
		[RenderAsUrl()]
		public string SocialThumbnail { get; set; }

		/// <summary>
		/// The value for the twitter:card tag.
		/// </summary>
		[RawValueOnly]
		public string TwitterCardType { get; set; }

		/// <summary>
		/// The value for the twitter:site tag.
		/// </summary>
		[RawValueOnly]
		public string TwitterSite { get; set; }

		/// <summary>
		/// The value for the twitter:creator tag.
		/// </summary>
		[RawValueOnly]
		public string TwitterCreator { get; set; }

		/// <summary>
		/// Indicates that if the mapped Item does not have social metatag values, they can be retrieved from an ancestor Item.
		/// </summary>
		public bool InheritTwitterValues { get; set; }
	}
}
