using Sitecore.Data.Fields;
using Sitecore.Web.UI.WebControls;
using System.Web;

namespace Constellation.Foundation.ModelMapping.FieldModels
{
	/// <summary>
	/// A Model that exposes all of the attributes of the General Link XML field type.
	/// </summary>
	public class GeneralLinkModel
	{
		internal GeneralLinkModel(LinkField field)
		{
			Anchor = field.Anchor;
			Class = field.Class;
			Target = field.Target;
			Text = field.Text;
			Title = field.Title;
			Url = GetUrlValue(field);
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
		/// The Url of the General Link field
		/// </summary>
		public string Url { get; }


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

			return field.Url;
		}
	}
}
