using Constellation.Foundation.ModelMapping.MappingAttributes;

namespace Constellation.Feature.PageTagging.Models
{
	/// <summary>
	/// A ViewModel that represents the common meta tags most SEO professionals like to see in an HTML document.
	/// </summary>
	public class PageMetadata
	{
		/// <summary>
		/// The contents of the html/head/title tag.
		/// </summary>
		[RawValueOnly]
		public string BrowserTitle { get; set; }

		/// <summary>
		/// The contents of the html/head/meta[@keywords] tag.
		/// </summary>
		[RawValueOnly]
		public string Keywords { get; set; }

		/// <summary>
		/// The contents of the html/head/meta[@description] tag.
		/// </summary>
		[RawValueOnly]
		public string MetaDescription { get; set; }

		/// <summary>
		/// The contents of the html/head/meta[@author] tag.
		/// </summary>
		[RawValueOnly]
		public string MetaAuthor { get; set; }

		/// <summary>
		/// The contents of the html/head/meta[@publisher] tag.
		/// </summary>
		[RawValueOnly]
		public string MetaPublisher { get; set; }

		/// <summary>
		/// Indicates whether the author and publisher values should be retrieved from an ancestor Item
		/// if they are not provided on the Item being mapped.
		/// </summary>
		public bool InheritAuthorAndPublisher { get; set; }

		/// <summary>
		/// Indicates whether this instance has a valid value for both Author and Publisher.
		/// </summary>
		public bool HasValidAuthorAndPublisher => !InheritAuthorAndPublisher || !string.IsNullOrEmpty(MetaAuthor) && !string.IsNullOrEmpty(MetaPublisher);

		/// <summary>
		/// Indicates whether this instance has a valid value for Author.
		/// </summary>
		public bool HasValidAuthor => !InheritAuthorAndPublisher || !string.IsNullOrEmpty(MetaAuthor);

		/// <summary>
		/// Indicates whether this instance has a valid value for Publisher.
		/// </summary>
		public bool HasValidPublisher => !InheritAuthorAndPublisher || !string.IsNullOrEmpty(MetaPublisher);
	}
}
