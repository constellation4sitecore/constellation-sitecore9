using System.Web;
using Constellation.Foundation.ModelMapping.MappingAttributes;

namespace Constellation.Feature.Navigation.Models
{
	/// <inheritdoc />
	/// <summary>
	/// A Navigation Link that displays a Sitecore Media Library asset.
	/// </summary>
	public class ImageNavigationLink : NavigationLink
	{
		/// <summary>
		/// The URL of the Media Library Item to display.
		/// </summary>
		[RenderAsUrl(false)]
		public string Image { get; set; }

		/// <summary>
		/// The Alt text of the Image
		/// </summary>
		public string ImageAlt { get; set; }

		/// <summary>
		/// The Width of the image, if that value is specified on the Image Navigation Link Item's Image field.
		/// </summary>
		public string ImageWidth { get; set; }

		/// <summary>
		/// The Height of the image, if that value is specified on the Image Navigation Link Item's Image field.
		/// </summary>
		public string ImageHeight { get; set; }

		/// <summary>
		/// If the target MediaItem is an SVG, this property will output the SVG XML to include inline.
		/// </summary>
		public HtmlString ImageSvg { get; set; }
	}
}
