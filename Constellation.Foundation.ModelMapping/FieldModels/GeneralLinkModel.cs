using Constellation.Foundation.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;
using System.Text;
using System.Web;

namespace Constellation.Foundation.ModelMapping.FieldModels
{
	/// <summary>
	/// A Model that exposes all of the attributes of the General Link XML field type.
	/// </summary>
	public class GeneralLinkModel
	{
		private readonly string _url;

		internal GeneralLinkModel(LinkField field)
		{
			Anchor = field.Anchor;
			Class = field.Class;
			QueryString = field.QueryString;
			Target = field.Target;
			Text = field.Text;
			Title = field.Title;
			_url = GetUrlValue(field);
			Rendered = new HtmlString(FieldRenderer.Render(field.InnerField.Item, field.InnerField.Name));
		}

		/// <summary>
		/// The Anchor property value of the General Link field
		/// </summary>
		public string Anchor { get; }

		/// <summary>
		/// The Class property value of the General Link field
		/// </summary>
		public string Class { get; }

		/// <summary>
		/// The Target property value of the General Link field
		/// </summary>
		public string Target { get; }

		/// <summary>
		/// The Text property value of the General Link field
		/// </summary>
		public string Text { get; }

		/// <summary>
		/// The Title property value of the General Link field
		/// </summary>
		public string Title { get; }

		/// <summary>
		/// The Url of the General Link field, including Querystring and Anchor if these properties have values.
		/// </summary>
		public string Url
		{
			get
			{
				var result = new StringBuilder(_url);

				if (!string.IsNullOrEmpty(QueryString) && !_url.Contains(QueryString))
				{
					if (!QueryString.StartsWith("?"))
					{
						result.Append("?");
					}

					result.Append(QueryString);
				}

				if (string.IsNullOrEmpty(Anchor) || _url.Contains(Anchor)) return result.ToString();

				if (!Anchor.StartsWith("#"))
				{
					result.Append("#");
				}

				result.Append(Anchor);

				return result.ToString();
			}
		}

		/// <summary>
		/// The QueryString of the General Link field
		/// </summary>
		public string QueryString { get; }



		/// <summary>
		/// The FieldRenderer output for the General Link field.
		/// </summary>
		public HtmlString Rendered { get; }

		private string GetUrlValue(LinkField field)
		{
			if (field.IsInternal)
			{
				return field.GetFriendlyUrl();
			}

			if (field.IsMediaLink)
			{
				var media = (MediaItem)field.TargetItem;
				return media.GetUrl();
			}

			return field.Url;
		}
	}
}
