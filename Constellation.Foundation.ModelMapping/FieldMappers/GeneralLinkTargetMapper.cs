using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class GeneralLinkTargetMapper : FieldAttributeMapper
	{
		protected override string GetPropertyName()
		{
			return base.GetPropertyName() + "Target";
		}

		protected override object GetValueToAssign()
		{
			LinkField field = Field;

			return field.Target;
		}
	}
}
