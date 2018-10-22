using Constellation.Foundation.ModelMapping.MappingAttributes;

namespace Constellation.Feature.PageAnalyticsScripts.Models
{
	/// <summary>
	/// A ViewModel representing Fields on an Item that implements the Page Analytics Scripts template in Sitecore.
	/// </summary>
	public class PageAnalyticsScriptsModel
	{
		/// <summary>
		/// Gets or sets any script tags that should be rendered in the html/header element of the document.
		/// </summary>
		[RawValueOnly]
		public string PageHeaderScript { get; set; }

		/// <summary>
		/// Gets or sets any script tags that should be rendered early in the html/body element of the document.
		/// </summary>
		[RawValueOnly]
		public string BodyTopScript { get; set; }

		/// <summary>
		/// Gets or sets any script tags that should be rendered just before the closing tag of the html/body element of the document.
		/// </summary>
		[RawValueOnly]
		public string BodyBottomScript { get; set; }
	}
}
