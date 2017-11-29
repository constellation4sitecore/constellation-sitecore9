using Constellation.Foundation.ModelMapping.MappingAttributes;

namespace Constellation.Feature.StaticNavigation.Models
{
	public class NavigationLink
	{
		[RenderAsUrl(false)]
		public string Link { get; set; }

		public bool UseThisDisplayName { get; set; }

		public string DisplayName { get; set; }
	}
}
