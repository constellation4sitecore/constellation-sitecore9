using Constellation.Foundation.ModelMapping.FieldModels;
using System.Web;

namespace Constellation.Feature.Navigation.Models
{
	/// <inheritdoc />
	/// <summary>
	/// A Navigation Link that displays a Sitecore Media Library asset.
	/// </summary>
	public class ImageNavigationLink : NavigationLink
	{
		/// <summary>
		/// The details of the Media Library Item to display
		/// </summary>
		public ImageModel Image { get; set; }

		/// <summary>
		/// If the target MediaItem is an SVG, this property will output the SVG XML to include inline.
		/// </summary>
		public HtmlString ImageSvg { get; set; }
	}
}
