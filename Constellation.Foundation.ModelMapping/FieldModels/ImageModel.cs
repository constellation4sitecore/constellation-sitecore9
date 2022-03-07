using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links.UrlBuilders;
using Sitecore.Resources.Media;
using Sitecore.Web.UI.WebControls;
using System.Web;
using System.Xml.Linq;

namespace Constellation.Foundation.ModelMapping.FieldModels
{
	/// <summary>
	/// When used as a ViewModel property, allows for the retrieval of multiple Image URLs on the View based
	/// on the needs of that specific view. this allows for any Features to be more generic and not expose View-specific
	/// facts like image size or Rendering parameters on the ViewModel.
	/// </summary>
	public class ImageModel
	{
		private readonly string _requestCacheKey;
		private readonly string _languageName;
		private readonly string _databaseName;
		private readonly ID _mediaID;

		internal ImageModel(ImageField field)
		{
			// populate the stock attributes
			this.Rendered = new HtmlString(FieldRenderer.Render(field.InnerField.Item, field.InnerField.Name));
			this.Alt = field.Alt;
			this.Height = field.Height;
			this.Width = field.Width;


			if (!string.IsNullOrEmpty(field.Value))
			{
				var element = XElement.Parse(field.Value);
				ContentHubContentType = GetAttributeValue(element, "stylelabs-content-type");
				Src = GetAttributeValue(element, "src");
				ThumbnailUrl = GetAttributeValue(element, "thumbnailsrc");
			}

			if (!IsContentHubContent && field.MediaItem != null)
			{
				// record facts to reconstitute MediaItem as necessary
				_languageName = field.InnerField.Item.Language.Name;
				_databaseName = field.InnerField.Item.Database.Name;
				_mediaID = field.MediaItem?.ID;
				_requestCacheKey = GetRequestCacheKeyValue(field.InnerField);

				// park the MediaItem in the Request cache
				MediaItem = field.MediaItem;

				// Generate the "stock" Media URL for this image.
				var options = new MediaUrlBuilderOptions()
				{
					Language = field.MediaItem.Language
				};

				if (int.TryParse(Width, out var width) && width > 0)
				{
					options.Width = width;
				}

				if (int.TryParse(Height, out var height) && height > 0)
				{
					options.Height = height;
				}

				var innerUrl = MediaManager.GetMediaUrl(field.MediaItem, options);
				Src = HashingUtils.ProtectAssetUrl(innerUrl);
			}
		}

		/// <summary>
		/// The Alt Text of the Image specified by this Field.
		/// </summary>
		public string Alt { get; }

		/// <summary>
		/// The default Src URL of the Image specified by this Field. Will obey any Height and Width parameters
		/// set via the Sitecore Image Field user interface.
		/// </summary>
		public string Src { get; }

		/// <summary>
		/// The Height value set on the Image Field
		/// </summary>
		public string Height { get; }

		/// <summary>
		/// The Width value set on the Image field
		/// </summary>
		public string Width { get; }

		/// <summary>
		/// True if the Image field references a ContentHub image
		/// </summary>
		public bool IsContentHubContent
		{
			get
			{
				return !string.IsNullOrEmpty(ContentHubContentType);
			}
		}

		/// <summary>
		/// Gets the StyleLabs content type.
		/// </summary>
		public string ContentHubContentType { get; }

		/// <summary>
		/// Ges the URL for the ContentHub thumbnail on the CDN
		/// </summary>
		public string ThumbnailUrl { get; }

		/// <summary>
		/// The result of calling FieldRenderer.Render() on the Image Field.
		/// </summary>
		public HtmlString Rendered { get; }

		/// <summary>
		/// The MediaItem that is the Value of the Field that was mapped to this Object.
		/// </summary>
		protected MediaItem MediaItem
		{
			get
			{
				if (IsContentHubContent)
				{
					return null;
				}


				if (HttpContext.Current != null)
				{
					if (string.IsNullOrEmpty(_requestCacheKey))
					{
						return null; // field was empty
					}

					return (MediaItem)HttpContext.Current.Items[_requestCacheKey];
				}

				var database = Sitecore.Configuration.Factory.GetDatabase(_databaseName);
				var language = Sitecore.Globalization.Language.Parse(_languageName);

				return database.GetItem(_mediaID, language);
			}
			private set
			{
				if (HttpContext.Current != null && !HttpContext.Current.Items.Contains(_requestCacheKey))
				{
					HttpContext.Current.Items.Add(_requestCacheKey, value);
				}
			}
		}

		/// <summary>
		/// Creates a URL that references a specific Transformed image in ContentHub
		/// </summary>
		/// <param name="transformationName">The name of the transformation as it appears in ContentHub</param>
		/// <returns>the transformed Src URL.</returns>
		public string GetContentHubTransformationSrc(string transformationName)
		{
			if (IsContentHubContent)
			{
				return Src + "&t=" + transformationName;
			}

			Log.Debug("ImageModel - ContentHub Transform URL requested for non-ContentHub content. Ignoring!", this);
			return Src;
		}

		/// <summary>
		/// Generate a new Image Src URL using the supplied Height parameter. The image will scale and remain within proportions.
		/// </summary>
		/// <param name="height">The new height of the image in pixels.</param>
		/// <returns>a URL to use in an image src attribute.</returns>
		public string GetCustomHeightImageSrc(int height)
		{
			if (IsContentHubContent)
			{
				Log.Debug("ImageModel - Media Library custom url requested for ContentHub content. Ignoring!", this);
				return string.Empty;
			}

			if (MediaItem == null)
			{
				return string.Empty;
			}

			var options = new MediaUrlBuilderOptions
			{
				Height = height,
				AllowStretch = false
			};

			var innerUrl = MediaManager.GetMediaUrl(MediaItem, options);
			return HashingUtils.ProtectAssetUrl(innerUrl);
		}

		/// <summary>
		/// Generate a new Image Src URL using the supplied Width parameter. The image will scale and remain within proportions.
		/// </summary>
		/// <param name="width">The new width of the image in pixels.</param>
		/// <returns>a URL to use in an image src attribute.</returns>
		public string GetCustomWidthImageSrc(int width)
		{
			if (IsContentHubContent)
			{
				Log.Debug("ImageModel - Media Library custom url requested for ContentHub content. Ignoring!", this);
				return string.Empty;
			}

			if (MediaItem == null)
			{
				return string.Empty;
			}

			var options = new MediaUrlBuilderOptions
			{
				Width = width,
				AllowStretch = false
			};

			var innerUrl = MediaManager.GetMediaUrl(MediaItem, options);
			return HashingUtils.ProtectAssetUrl(innerUrl);
		}

		/// <summary>
		/// Generate a new Image Src URL using the supplied parameters.
		/// </summary>
		/// <param name="width">the new image width in pixels</param>
		/// <param name="height">the new image height in pixels</param>
		/// <param name="allowStretch">Whether to stretch the image to fit the new dimensions</param>
		/// <param name="ignoreAspectRatio">whether the image's aspect ratio should be respected</param>
		/// <returns>a URL to use in an image src attribute.</returns>
		public string GetCustomImageSrc(int width, int height, bool allowStretch = false, bool ignoreAspectRatio = false)
		{
			if (IsContentHubContent)
			{
				Log.Debug("ImageModel - Media Library custom url requested for ContentHub content. Ignoring!", this);
				return string.Empty;
			}

			if (MediaItem == null)
			{
				return string.Empty;
			}

			var options = new MediaUrlBuilderOptions
			{
				Width = width,
				Height = height,
				AllowStretch = allowStretch,
				IgnoreAspectRatio = ignoreAspectRatio
			};

			var innerUrl = MediaManager.GetMediaUrl(MediaItem, options);
			return HashingUtils.ProtectAssetUrl(innerUrl);
		}

		/// <summary>
		/// Gets the string value of an XML attribute if it exists.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="attributeName"></param>
		/// <returns></returns>
		private static string GetAttributeValue(XElement element, string attributeName)
		{
			var attribute = element.Attribute(attributeName);

			return attribute?.Value;
		}

		private static string GetRequestCacheKeyValue(Field field)
		{
			return $"imageModel{field.Item.ID.ToShortID()}{field.ID.ToShortID()}";
		}
	}
}
