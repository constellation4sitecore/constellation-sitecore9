using System.IO;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	/// <inheritdoc />
	/// <summary>
	/// Given an Image field, renders the associated SVG file as a string to a Model Property suffixed with the word "Svg".
	/// </summary>
	public class ImageSvgMapper : FieldAttributeMapper
	{
		/// <inheritdoc />
		protected override string GetPropertyName()
		{
			return base.GetPropertyName() + "Svg";
		}

		/// <inheritdoc />
		protected override object GetValueToAssign()
		{
			return RenderSvgContents();
		}

		private string RenderSvgContents()
		{
			ImageField field = this.Field;

			if (field.MediaID.IsGlobalNullId)
			{
				Log.Debug("ImageSvgMapper: MediaID was null.");
				return string.Empty;
			}

			if (field.MediaItem == null)
			{
				Log.Debug("ImageSvgMapper: MediaItem was null.");
				return string.Empty;
			}

			var mediaItem = new MediaItem(field.MediaItem);

			if (mediaItem.MimeType != "image/svg+xml")
			{
				Log.Debug("ImageSvgMapper: MimeType was not image/svg+xml.");
				return string.Empty;
			}

			using (var reader = new StreamReader(MediaManager.GetMedia(mediaItem).GetStream().Stream))
			{
				Log.Debug("ImageSvgMapper: reading Media Stream to Model.");
				return reader.ReadToEnd();
			}
		}
	}
}
