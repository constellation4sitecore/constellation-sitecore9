using Constellation.Foundation.ModelMapping.MappingAttributes;

namespace Constellation.Feature.PageAnalyticsScripts.Models
{
	public class PageAnalyticsScriptsModel
	{
		[RawValueOnly]
		public string PageHeaderScript { get; set; }

		[RawValueOnly]
		public string BodyTopScript { get; set; }

		[RawValueOnly]
		public string BodyBottomScript { get; set; }
	}
}
