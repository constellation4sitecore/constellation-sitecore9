namespace Constellation.Foundation.Items.FieldProperties
{
	using Sitecore.Data.Fields;

	using System.Diagnostics.CodeAnalysis;

	/// <inheritdoc />
	/// <summary>
	/// Wraps a Sitecore GroupedDroplistField
	/// </summary>
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
	public class GroupedDroplistProperty : ValueLookupProperty
	{
		#region Constructors
		/// <inheritdoc />
		/// <summary>
		/// Initializes a new instance of the <see cref="GroupedDroplistProperty"/> class.
		/// </summary>
		/// <param name="field">The field to wrap.</param>
		public GroupedDroplistProperty(Field field)
			: base(field)
		{
			// Nothing to do.
		}
		#endregion

		#region Operators
		/// <summary>
		/// Allows interoperability with Sitecore GroupedDroplistField.
		/// </summary>
		/// <param name="field">The field to wrap.</param>
		/// <returns>A new instance of GroupedDroplistProperty using the supplied field.</returns>
		public static implicit operator GroupedDroplistProperty(GroupedDroplistField field)
		{
			return new GroupedDroplistProperty(field.InnerField);
		}

		/// <summary>
		/// Allows interoperability with Sitecore GroupedDroplistField.
		/// </summary>
		/// <param name="property">The property to convert.</param>
		/// <returns>The property.InnerField.</returns>
		public static implicit operator GroupedDroplistField(GroupedDroplistProperty property)
		{
			return property.InnerField;
		}
		#endregion
	}
}
