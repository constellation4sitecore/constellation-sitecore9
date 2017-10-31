namespace Constellation.Foundation.Items.FieldProperties
{
	using Sitecore.Data.Fields;

	using System.Diagnostics.CodeAnalysis;

	/// <inheritdoc />
	/// <summary>
	/// Wraps a Sitecore GroupedDroplinkField
	/// </summary>
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Name of Sitecore field.")]
	public class GroupedDroplinkProperty : LookupProperty
	{
		#region Constructors
		/// <inheritdoc />
		/// <summary>
		/// Initializes a new instance of the <see cref="GroupedDroplinkProperty"/> class.
		/// </summary>
		/// <param name="field">The field to wrap.</param>
		public GroupedDroplinkProperty(Field field)
			: base(field)
		{
			// nothing to do!
		}
		#endregion

		#region Operators
		/// <summary>
		/// Allows interoperability with Sitecore GroupedDroplinkField.
		/// </summary>
		/// <param name="field">The field to wrap.</param>
		/// <returns>A new instance of GroupedDroplinkProperty using the supplied field.</returns>
		public static implicit operator GroupedDroplinkProperty(GroupedDroplinkField field)
		{
			return new GroupedDroplinkProperty(field.InnerField);
		}

		/// <summary>
		/// Allows interoperability with Sitecore GroupedDroplinkField.
		/// </summary>
		/// <param name="property">The property to convert.</param>
		/// <returns>The property.InnerField.</returns>
		public static implicit operator GroupedDroplinkField(GroupedDroplinkProperty property)
		{
			return property.InnerField;
		}
		#endregion
	}
}
