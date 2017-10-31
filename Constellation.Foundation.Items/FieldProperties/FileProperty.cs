namespace Constellation.Foundation.Items.FieldProperties
{
	using Sitecore.Data;
	using Sitecore.Data.Fields;
	using Sitecore.Data.Items;
	using Sitecore.Globalization;
	using Sitecore.Links;

	/// <inheritdoc />
	/// <summary>
	/// Facade for Sitecore File Field
	/// </summary>
	public class FileProperty : XmlProperty
	{
		/// <summary>
		/// The file field to wrap.
		/// </summary>
		private readonly FileField fileField;

		#region Constructors
		/// <inheritdoc />
		/// <summary>
		/// Initializes a new instance of the <see cref="FileProperty"/> class.
		/// </summary>
		/// <param name="field">The field to wrap.</param>
		public FileProperty(Field field)
			: base(field)
		{
			this.fileField = field;
		}

		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the database containing the the media.
		/// </summary>
		/// <value>
		/// The media database.
		/// </value>
		/// <contract><requires name="value" condition="not null"/><ensures condition="not null"/></contract>
		public Database MediaDatabase
		{
			get { return this.fileField.MediaDatabase; }
			set { this.fileField.MediaDatabase = value; }
		}

		/// <summary>
		/// Gets or sets the language of the media item to use. 
		/// </summary>
		/// <value>
		/// The media language.
		/// </value>
		/// <contract><requires name="value" condition="not null"/><ensures condition="not null"/></contract>
		public Language MediaLanguage
		{
			get { return this.fileField.MediaLanguage; }
			set { this.fileField.MediaLanguage = value; }
		}

		/// <summary>
		/// Gets or sets the ID of the media item.
		/// </summary>
		/// <value>
		/// The media ID.
		/// </value>
		/// <contract><requires name="value" condition="not null"/><ensures condition="not null"/></contract>
		public ID MediaID
		{
			get { return this.fileField.MediaID; }
			set { this.fileField.MediaID = value; }
		}

		/// <summary>
		/// Gets the media item.
		/// </summary>
		/// <value>
		/// The media item.
		/// </value>
		public Item MediaItem
		{
			get { return this.fileField.MediaItem; }
		}

		/// <summary>
		/// Gets or sets the version of the media item to use.
		/// </summary>
		/// <value>
		/// The media version.
		/// </value>
		/// <contract><requires name="value" condition="not null"/><ensures condition="nullable"/></contract>
		public Version MediaVersion
		{
			get { return this.fileField.MediaVersion; }
			set { this.fileField.MediaVersion = value; }
		}

		/// <summary>
		/// Gets or sets the source path.
		/// </summary>
		/// <value>
		/// The source.
		/// </value>
		/// <contract><requires name="value" condition="not null"/><ensures condition="not null"/></contract>
		public string Src
		{
			get { return this.fileField.Src; }
			set { this.fileField.Src = value; }
		}
		#endregion

		#region Operators
		/// <summary>
		/// Allows interoperability with Sitecore FileField.
		/// </summary>
		/// <param name="field">The field to wrap.</param>
		/// <returns>A new instance of FileProperty based on the supplied field.</returns>
		public static implicit operator FileProperty(FileField field)
		{
			return new FileProperty(field.InnerField);
		}

		/// <summary>
		/// Allows interoperability with Sitecore FileField.
		/// </summary>
		/// <param name="property">The property to convert.</param>
		/// <returns>The property.InnerField.</returns>
		public static implicit operator FileField(FileProperty property)
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
			this.fileField.Relink(itemLink, newLink);
		}

		/// <summary>
		/// Removes the link.
		/// </summary>
		/// <param name="itemLink">The item link.</param><contract><requires name="itemLink" condition="not null"/></contract>
		public override void RemoveLink(ItemLink itemLink)
		{
			this.fileField.RemoveLink(itemLink);
		}

		/// <summary>
		/// Updates the link.
		/// </summary>
		/// <param name="itemLink">The link.</param><contract><requires name="itemLink" condition="not null"/></contract>
		public override void UpdateLink(ItemLink itemLink)
		{
			this.fileField.UpdateLink(itemLink);
		}

		/// <summary>
		/// Validates the links.
		/// </summary>
		/// <param name="result">The result.</param><contract><requires name="result" condition="not null"/></contract>
		public override void ValidateLinks(LinksValidationResult result)
		{
			this.fileField.ValidateLinks(result);
		}
		#endregion
	}
}
