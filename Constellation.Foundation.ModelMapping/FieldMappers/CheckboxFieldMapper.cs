using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class CheckboxFieldMapper : FieldMapper<bool>
	{
		protected override bool ExtractTypedValueFromField()
		{
			return ((CheckboxField)Field).Checked;
		}
	}
}
