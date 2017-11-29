using Constellation.Foundation.ModelMapping.MappingAttributes;

namespace Constellation.Feature.StaticNavigation.Models
{
	public class ImageNavigationLink : NavigationLink
	{
		[RenderAsUrl(false)]
		public string Image { get; set; }
	}
}
