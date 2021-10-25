using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links.UrlBuilders;
using Sitecore.Resources.Media;
using Sitecore.Web.UI.WebControls;
using System.Web;

namespace Constellation.Foundation.ModelMapping.FieldModels
{
	/// <summary>
	/// When used as a ViewModel property, allows for the retrieval of File field values on the View based
	/// on the needs of that specific view. 
	/// </summary>
	public class FileModel
	{
		private readonly string _requestCacheKey;
		private readonly string _languageName;
		private readonly string _databaseName;
		private readonly ID _mediaID;

		internal FileModel(FileField field)
		{
			// record facts to reconstitute MediaItem as necessary
			_languageName = field.InnerField.Item.Language.Name;
			_databaseName = field.InnerField.Item.Database.Name;
			_mediaID = field.MediaItem?.ID;

			_requestCacheKey = GetRequestCacheKeyValue(field.InnerField);
			// park the MediaItem in the Request cache
			MediaItem = field.MediaItem;

			// populate the stock attributes
			this.Rendered = new HtmlString(FieldRenderer.Render(field.InnerField.Item, field.InnerField.Name));

			var file = field.MediaItem;
			this.Name = file.Name;
			this.DisplayName = file.DisplayName;

			if (file != null)
			{

				var options = new MediaUrlBuilderOptions()
				{
					Language = file.Language
				};

				var innerUrl = MediaManager.GetMediaUrl(file, options);
				Src = HashingUtils.ProtectAssetUrl(innerUrl);
			}
			else
			{
				Src = string.Empty;
			}
		}

		/// <summary>
		/// The MediaItem's Display Name
		/// </summary>
		public string DisplayName { get; }

		/// <summary>
		/// The MediaItem's Name
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// The Url of the file using the current media context.
		/// </summary>
		public string Src { get; }

		/// <summary>
		/// The Url of the file using the current media context.
		/// </summary>
		public string Url
		{
			get
			{
				return Src;
			}
		}

		/// <summary>
		/// The result of calling FieldRenderer.Render() on the File Field.
		/// </summary>
		public HtmlString Rendered { get; }

		/// <summary>
		/// The MediaItem that is the Value of the Field that was mapped to this Object.
		/// </summary>
		protected MediaItem MediaItem
		{
			get
			{
				if (HttpContext.Current != null)
				{
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

		private static string GetRequestCacheKeyValue(Field field)
		{
			return $"fileUrlBuilder{field.Item.ID.ToShortID()}{field.ID.ToShortID()}";
		}
	}
}
