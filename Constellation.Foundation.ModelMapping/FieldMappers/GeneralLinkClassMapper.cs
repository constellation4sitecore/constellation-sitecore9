using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class GeneralLinkClassMapper : FieldAttributeMapper
	{
		protected override string GetPropertyName()
		{
			return base.GetPropertyName() + "Class";
		}

		protected override object GetValueToAssign()
		{
			LinkField field = Field;

			return field.Class;
		}
	}
}
