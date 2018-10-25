using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	/// <inheritdoc />
	/// <summary>
	/// Given any of Sitecore's text fields, write the Value to the Model's Property.
	/// </summary>
	public class TextFieldMapper : FieldMapper<string>
	{
		/// <inheritdoc />
		protected override string ExtractTypedValueFromField()
		{
			return Field.Value; // This will never get called, since raw values are handled via Attributes.
		}
	}
}
