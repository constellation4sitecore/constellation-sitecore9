namespace Constellation.Foundation.Items.FieldProperties
{
	using Sitecore.Data;
	using Sitecore.Data.Fields;
	using Sitecore.Data.Items;
	using Sitecore.Globalization;
	using Sitecore.Links;

	/// <inheritdoc />
	/// <summary>
	/// Wraps a Sitecore ImageField
	/// </summary>
	public class ImageProperty : XmlProperty
	{
		/// <summary>
		/// The Image Field to wrap.
		/// </summary>
		private readonly ImageField imageField;

		#region Constructors
		/// <inheritdoc />
		/// <summary>
		/// Initializes a new instance of the <see cref="ImageProperty"/> class.
		/// </summary>
		/// <param name="field">The field to wrap.</param>
		public ImageProperty(Field field)
			: base(field)
		{
			this.imageField = field;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the Alt text.
		/// </summary>
		/// <value>
		/// The alt text.
		/// </value>
		public string Alt
		{
			get { return this.imageField.Alt; }
			set { this.imageField.Alt = value; }
		}

		/// <summary>
		/// Gets or sets the Border value.
		/// </summary>
		/// <value>
		/// The Border value.
		/// </value>
		public string Border
		{
			get { return this.imageField.Border; }
			set { this.imageField.Border = value; }
		}

		/// <summary>
		/// Gets or sets the HTML class.
		/// </summary>
		/// <value>
		/// The class.
		/// </value>
		public string Class
		{
			get { return this.imageField.Class; }
			set { this.imageField.Class = value; }
		}

		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		/// <value>
		/// The height.
		/// </value>
		public string Height
		{
			get { return this.imageField.Height; }
			set { this.imageField.Height = value; }
		}

		/// <summary>
		/// Gets or sets the HSpace value.
		/// </summary>
		/// <value>
		/// The HSpace value.
		/// </value>
		public string HSpace
		{
			get { return this.imageField.HSpace; }
			set { this.imageField.HSpace = value; }
		}

		/// <summary>
		/// Gets a value indicating whether the image is internal.
		/// </summary>
		/// <value>
		/// <c>true</c> if the image is internal; otherwise, <c>false</c>.
		/// </value>
		public bool IsInternal
		{
			get { return this.imageField.IsInternal; }
		}

		/// <summary>
		/// Gets or sets the link type.
		/// </summary>
		/// <value>
		/// The type of the link.
		/// </value>
		public string LinkType
		{
			get { return this.imageField.LinkType; }
			set { this.imageField.LinkType = value; }
		}

		/// <summary>
		/// Gets or sets the database containing the the media.
		/// </summary>
		/// <value>
		/// The media database.
		/// </value>
		public Database MediaDatabase
		{
			get { return this.imageField.MediaDatabase; }
			set { this.imageField.MediaDatabase = value; }
		}

		/// <summary>
		/// Gets or sets the ID of the media item.
		/// </summary>
		/// <value>
		/// The media ID.
		/// </value>
		public ID MediaID
		{
			get { return this.imageField.MediaID; }
			set { this.imageField.MediaID = value; }
		}

		/// <summary>
		/// Gets the media item.
		/// </summary>
		/// <value>
		/// The media item.
		/// </value>
		public Item MediaItem
		{
			get { return this.imageField.MediaItem; }
		}

		/// <summary>
		/// Gets or sets the language of the media item to use.
		/// </summary>
		/// <value>
		/// The media language.
		/// </value>
		public Language MediaLanguage
		{
			get { return this.imageField.MediaLanguage; }
			set { this.imageField.MediaLanguage = value; }
		}

		/// <summary>
		/// Gets or sets the version of the media item to use.
		/// </summary>
		/// <value>
		/// The media version.
		/// </value>
		public Version MediaVersion
		{
			get { return this.imageField.MediaVersion; }
			set { this.imageField.MediaVersion = value; }
		}

		/// <summary>
		/// Gets or sets the VSpace value.
		/// </summary>
		/// <value>
		/// The VSpace value.
		/// </value>
		public string VSpace
		{
			get { return this.imageField.VSpace; }
			set { this.imageField.VSpace = value; }
		}

		/// <summary>
		/// Gets or sets the width.
		/// </summary>
		/// <value>
		/// The width.
		/// </value>
		public string Width
		{
			get { return this.imageField.Width; }
			set { this.imageField.Width = value; }
		}
		#endregion

		#region Operators
		/// <summary>
		/// Allows interoperability with Sitecore ImageField.
		/// </summary>
		/// <param name="field">The field to wrap.</param>
		/// <returns>A new instance of ImageProperty using the supplied field.</returns>
		public static implicit operator ImageProperty(ImageField field)
		{
			return new ImageProperty(field.InnerField);
		}

		/// <summary>
		/// Allows interoperability with Sitecore ImageField.
		/// </summary>
		/// <param name="property">The property to convert.</param>
		/// <returns>The property.InnerField.</returns>
		public static implicit operator ImageField(ImageProperty property)
		{
			return property.InnerField;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Relinks the specified item.
		/// </summary>
		/// <param name="itemLink">The item link.</param><param name="newLink">The new link.</param><contract><requires name="itemLink" condition="not null"/><requires name="newLink" condition="not null"/></contract>
		public override void Relink(ItemLink itemLink, Item newLink)
		{
			this.imageField.Relink(itemLink, newLink);
		}

		/// <summary>
		/// Removes the link.
		/// </summary>
		/// <param name="itemLink">The item link.</param><contract><requires name="itemLink" condition="not null"/></contract>
		public override void RemoveLink(ItemLink itemLink)
		{
			this.imageField.RemoveLink(itemLink);
		}

		/// <summary>
		/// Updates the link.
		/// </summary>
		/// <param name="itemLink">The link.</param><contract><requires name="itemLink" condition="not null"/></contract>
		public override void UpdateLink(ItemLink itemLink)
		{
			this.imageField.UpdateLink(itemLink);
		}

		/// <summary>
		/// Validates the links.
		/// </summary>
		/// <param name="result">The result.</param><contract><requires name="result" condition="not null"/></contract>
		public override void ValidateLinks(LinksValidationResult result)
		{
			this.imageField.ValidateLinks(result);
		}
		#endregion
	}
}
