namespace Constellation.Foundation.Items.FieldProperties
{
	using Sitecore.Data;
	using Sitecore.Data.Fields;
	using Sitecore.Data.Items;
	using Sitecore.Links;

	/// <inheritdoc />
	/// <summary>
	/// Wraps a Sitecore LookupField
	/// </summary>
	public class LookupProperty : FieldProperty
	{
		/// <summary>
		/// The field to wrap.
		/// </summary>
		private readonly LookupField _lookupField;

		#region Constructors
		/// <inheritdoc />
		/// <summary>
		/// Initializes a new instance of the <see cref="LookupProperty"/> class.
		/// </summary>
		/// <param name="field">The field to wrap.</param>
		public LookupProperty(Field field)
			: base(field)
		{
			_lookupField = field;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the target ID.
		/// </summary>
		/// <value>
		/// The target ID.
		/// </value>
		/// <contract><ensures condition="not null"/></contract>
		public ID TargetID => _lookupField.TargetID;

		/// <summary>
		/// Gets the target item.
		/// </summary>
		/// <value>
		/// The target item.
		/// </value>
		public Item TargetItem => _lookupField.TargetItem;

		#endregion

		#region Operators
		/// <summary>
		/// Allows interoperability with Sitecore LookupField.
		/// </summary>
		/// <param name="field">The field to wrap.</param>
		/// <returns>A new instance of LookupProperty using the supplied field.</returns>
		public static implicit operator LookupProperty(LookupField field)
		{
			return new LookupProperty(field.InnerField);
		}

		/// <summary>
		/// Allows interoperability with Sitecore LookupField.
		/// </summary>
		/// <param name="property">The property to convert.</param>
		/// <returns>The property.InnerField.</returns>
		public static implicit operator ImageField(LookupProperty property)
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
			_lookupField.Relink(itemLink, newLink);
		}

		/// <summary>
		/// Removes the link.
		/// </summary>
		/// <param name="itemLink">The item link.</param><contract><requires name="itemLink" condition="not null"/></contract>
		public override void RemoveLink(ItemLink itemLink)
		{
			_lookupField.RemoveLink(itemLink);
		}

		/// <summary>
		/// Validates the links. 
		/// </summary>
		/// <param name="result">The result.</param><contract><requires name="result" condition="not null"/></contract>
		public override void ValidateLinks(LinksValidationResult result)
		{
			_lookupField.ValidateLinks(result);
		}
		#endregion
	}
}
