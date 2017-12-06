using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class GeneralLinkTextMapper : FieldAttributeMapper
	{
		protected override string GetPropertyName()
		{
			return base.GetPropertyName() + "Text";
		}

		protected override object GetValueToAssign()
		{
			LinkField field = Field;

			return field.Text;
		}
	}
}
