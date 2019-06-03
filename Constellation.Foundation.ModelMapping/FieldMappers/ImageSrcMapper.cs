using Sitecore.Data.Fields;
using Sitecore.Resources.Media;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	/// <inheritdoc />
	/// <summary>
	/// Maps the MediaManager URL of an Image field to a Model Property suffixed with the word "Src".
	/// </summary>
	public class ImageSrcMapper : FieldAttributeMapper
	{
		/// <inheritdoc />
		protected override string GetPropertyName()
		{
			return base.GetPropertyName() + "Src";
		}

		/// <inheritdoc />
		protected override object GetValueToAssign()
		{
			return GetUrlFromField();
		}

		private string GetUrlFromField()
		{
			ImageField field = Field;
			var targetImage = field.MediaItem;

			if (targetImage != null)
			{

				var options = new MediaUrlOptions()
				{
					Language = targetImage.Language
				};

				int width;
				if (int.TryParse(field.Width, out width) && width > 0)
				{
					options.Width = width;
				}

				int height;
				if (int.TryParse(field.Height, out height) && height > 0)
				{
					options.Height = height;
				}

				var innerUrl = MediaManager.GetMediaUrl(targetImage, options);
				var url = HashingUtils.ProtectAssetUrl(innerUrl);

				return url;
			}

			return string.Empty;
		}
	}
}
