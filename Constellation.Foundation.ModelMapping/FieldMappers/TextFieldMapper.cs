using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class TextFieldMapper : FieldMapper<string>
	{
		protected override string ExtractTypedValueFromField()
		{
			return Field.Value; // This will never get called, since raw values are handled via Attributes.
		}
	}
}
