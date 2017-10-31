namespace Constellation.Foundation.Items.FieldProperties
{
	using Sitecore.Data.Fields;
	using Sitecore.Data.Items;
	using Sitecore.Links;

	/// <inheritdoc />
	/// <summary>
	/// Wraps a Sitecore ValueLookupField
	/// </summary>
	public class ValueLookupProperty : FieldProperty
	{
		/// <summary>
		/// The field to wrap.
		/// </summary>
		private readonly ValueLookupField _valueLookupField;

		#region Constructors
		/// <inheritdoc />
		/// <summary>
		/// Initializes a new instance of the <see cref="ValueLookupProperty"/> class.
		/// </summary>
		/// <param name="field">The field to wrap.</param>
		public ValueLookupProperty(Field field)
			: base(field)
		{
			this._valueLookupField = field;
		}
		#endregion

		#region Operators
		/// <summary>
		/// Allows interoperability with Sitecore ValueLookupField.
		/// </summary>
		/// <param name="field">The field to wrap.</param>
		/// <returns>A new instance of ValueLookupProperty using the supplied field.</returns>
		public static implicit operator ValueLookupProperty(ValueLookupField field)
		{
			return new ValueLookupProperty(field.InnerField);
		}

		/// <summary>
		/// Allows interoperability with Sitecore ValueLookupField.
		/// </summary>
		/// <param name="property">The property to convert.</param>
		/// <returns>The property.InnerField.</returns>
		public static implicit operator ValueLookupField(ValueLookupProperty property)
		{
			return property.InnerField;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Relinks the specified item.
		/// </summary>
		/// <param name="itemLink">The item link.</param><param name="newLink">The new link.</param>
		public override void Relink(ItemLink itemLink, Item newLink)
		{
			this._valueLookupField.Relink(itemLink, newLink);
		}

		/// <summary>
		/// Removes the link.
		/// </summary>
		/// <param name="itemLink">The item link.</param>
		public override void RemoveLink(ItemLink itemLink)
		{
			this._valueLookupField.RemoveLink(itemLink);
		}

		/// <summary>
		/// Updates the link.
		/// </summary>
		/// <param name="itemLink">The link.</param>
		public override void UpdateLink(ItemLink itemLink)
		{
			this._valueLookupField.UpdateLink(itemLink);
		}

		/// <summary>
		/// Validates the links.
		/// </summary>
		/// <param name="result">The result.</param>
		public override void ValidateLinks(LinksValidationResult result)
		{
			this._valueLookupField.ValidateLinks(result);
		}
		#endregion
	}
}
