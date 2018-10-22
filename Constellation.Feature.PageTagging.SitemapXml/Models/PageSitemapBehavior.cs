using Constellation.Feature.PageTagging.Models;

namespace Constellation.Feature.PageTagging.SitemapXml.Models
{
	/// <inheritdoc />
	public class PageSitemapBehavior : PageSearchEngineDirectives
	{
		/// <summary>
		/// Gets or sets a value indicating whether the Item should be included in the sitemap.xml document.
		/// </summary>
		public bool IncludeInSitemap { get; set; }

		/// <summary>
		/// Gets or sets a value representing the change frequency of the Item relative to the sitemap.xml document.
		/// </summary>
		public ChangeFrequency ChangeFrequency { get; set; }
	}
}
