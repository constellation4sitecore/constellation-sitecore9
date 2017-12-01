using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class GeneralLinkTitleMapper : FieldAttributeMapper
	{
		protected override string GetPropertyName()
		{
			return base.GetPropertyName() + "Title";
		}

		protected override object GetValueToAssign()
		{
			LinkField field = Field;

			return field.Target;
		}
	}
}
