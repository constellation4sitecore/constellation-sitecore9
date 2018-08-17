using Constellation.Feature.PageTagging.Models;

namespace Constellation.Feature.PageTagging.SitemapXml.Models
{
	public class PageSitemapBehavior : PageSearchEngineDirectives
	{
		public bool IncludeInSitemap { get; set; }

		public ChangeFrequency ChangeFrequency { get; set; }
	}
}
