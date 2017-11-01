namespace Constellation.Foundation.Items.FieldProperties
{
	using Sitecore.Data.Fields;
	using Sitecore.Data.Items;
	using Sitecore.Links;

	using System.Collections.Generic;

	/// <inheritdoc />
	/// <summary>
	/// Wraps a Sitecore HtmlField.
	/// </summary>
	public class HtmlProperty : FieldProperty
	{
		/// <summary>
		/// The HtmlField to wrap.
		/// </summary>
		private readonly HtmlField _htmlField;

		#region Constructors
		/// <inheritdoc />
		/// <summary>
		/// Initializes a new instance of the <see cref="HtmlProperty"/> class.
		/// </summary>
		/// <param name="field">The field to wrap.</param>
		public HtmlProperty(Field field)
			: base(field)
		{
			_htmlField = field;
		}
		#endregion

		#region Operators
		/// <summary>
		/// Allows interoperability with Sitecore HtmlField.
		/// </summary>
		/// <param name="field">The field to wrap.</param>
		/// <returns>A new instance of HtmlProperty using the supplied field.</returns>
		public static implicit operator HtmlProperty(HtmlField field)
		{
			return new HtmlProperty(field.InnerField);
		}

		/// <summary>
		/// Allows interoperability with Sitecore HtmlField.
		/// </summary>
		/// <param name="property">The property to convert.</param>
		/// <returns>The property.InnerField.</returns>
		public static implicit operator HtmlField(HtmlProperty property)
		{
			return property.InnerField;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Tightens the relative image links.
		/// </summary>
		/// <param name="html">The HTML.</param>
		/// <returns>
		/// The relative image links.
		/// </returns>
		/// <contract><requires name="html" condition="not empty"/><ensures condition="not null"/></contract>
		public static string TightenRelativeImageLinks(string html)
		{
			return HtmlField.TightenRelativeImageLinks(html);
		}

		/// <summary>
		/// Gets the plain text.
		/// </summary>
		/// <returns>The value stripped of all HTML elements.</returns>
		public virtual string GetPlainText()
		{
			return _htmlField.GetPlainText();
		}

		/// <summary>
		/// Gets the web edit buttons.
		/// </summary>
		/// <returns>
		/// The web edit buttons.
		/// </returns>
		/// <contract><ensures condition="nullable"/></contract>
		public override List<WebEditButton> GetWebEditButtons()
		{
			return _htmlField.GetWebEditButtons();
		}

		/// <summary>
		/// Relinks the specified item.
		/// </summary>
		/// <param name="itemLink">The item link.</param><param name="newLink">The new link.</param><contract><requires name="itemLink" condition="not null"/><requires name="newLink" condition="not null"/></contract>
		public override void Relink(ItemLink itemLink, Item newLink)
		{
			_htmlField.Relink(itemLink, newLink);
		}

		/// <summary>
		/// Removes the link.
		/// </summary>
		/// <param name="itemLink">The item link.</param><contract><requires name="itemLink" condition="not null"/></contract>
		public override void RemoveLink(ItemLink itemLink)
		{
			_htmlField.RemoveLink(itemLink);
		}

		/// <summary>
		/// Validates the links.
		/// </summary>
		/// <param name="result">The result.</param><contract><requires name="result" condition="not null"/></contract>
		public override void ValidateLinks(LinksValidationResult result)
		{
			_htmlField.ValidateLinks(result);
		}
		#endregion
	}
}
