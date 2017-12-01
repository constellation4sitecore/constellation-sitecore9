using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class GeneralLinkAnchorMapper : FieldAttributeMapper
	{
		protected override string GetPropertyName()
		{
			return base.GetPropertyName() + "Anchor";
		}

		protected override object GetValueToAssign()
		{
			LinkField field = Field;

			return field.Target;
		}
	}
}
