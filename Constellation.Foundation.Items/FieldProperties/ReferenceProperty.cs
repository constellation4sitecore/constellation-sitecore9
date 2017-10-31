namespace Constellation.Foundation.Items.FieldProperties
{
	using Sitecore.Data;
	using Sitecore.Data.Fields;
	using Sitecore.Data.Items;
	using Sitecore.Links;

	/// <inheritdoc />
	/// <summary>
	/// Wraps a Sitecore ReferenceField
	/// </summary>
	public class ReferenceProperty : FieldProperty
	{
		/// <summary>
		/// The field to wrap.
		/// </summary>
		private readonly ReferenceField _referenceField;

		#region Constructors
		/// <inheritdoc />
		/// <summary>
		/// Initializes a new instance of the <see cref="ReferenceProperty"/> class.
		/// </summary>
		/// <param name="field">The field to wrap.</param>
		public ReferenceProperty(Field field)
			: base(field)
		{
			this._referenceField = field;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the path to the target.
		/// </summary>
		/// <value>
		/// The path.
		/// </value>
		public string Path
		{
			get { return this._referenceField.Path; }
			set { this._referenceField.Path = value; }
		}

		/// <summary>
		/// Gets the target ID.
		/// </summary>
		/// <value>
		/// The target ID.
		/// </value>
		public ID TargetID
		{
			get { return this._referenceField.TargetID; }
		}

		/// <summary>
		/// Gets the target item.
		/// </summary>
		/// <value>
		/// The target item.
		/// </value>
		public Item TargetItem
		{
			get { return this._referenceField.TargetItem; }
		}
		#endregion

		#region Operators
		/// <summary>
		/// Allows interoperability with Sitecore ReferenceField.
		/// </summary>
		/// <param name="field">The field to wrap.</param>
		/// <returns>A new instance of ReferenceProperty using the supplied field.</returns>
		public static implicit operator ReferenceProperty(ReferenceField field)
		{
			return new ReferenceProperty(field.InnerField);
		}

		/// <summary>
		/// Allows interoperability with Sitecore ReferenceField.
		/// </summary>
		/// <param name="property">The property to convert.</param>
		/// <returns>The property.InnerField.</returns>
		public static implicit operator ReferenceField(ReferenceProperty property)
		{
			return property.InnerField;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Relinks the specified item.
		/// </summary>
		/// <param name="itemLink">The item link.</param>
		/// <param name="newLink">The new link.
		/// </param>
		public override void Relink(ItemLink itemLink, Item newLink)
		{
			this._referenceField.Relink(itemLink, newLink);
		}

		/// <summary>
		/// Removes the link.
		/// </summary>
		/// <param name="itemLink">The item link.</param>
		public override void RemoveLink(ItemLink itemLink)
		{
			this._referenceField.RemoveLink(itemLink);
		}

		/// <summary>
		/// Validates the links.
		/// </summary>
		/// <param name="result">The result.</param>
		public override void ValidateLinks(LinksValidationResult result)
		{
			this._referenceField.ValidateLinks(result);
		}
		#endregion
	}
}
