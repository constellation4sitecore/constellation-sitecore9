using Constellation.Foundation.Data;
using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class GeneralLinkUrlMapper : FieldAttributeMapper
	{
		protected override string GetPropertyName()
		{
			return base.GetPropertyName() + "Url";
		}

		protected override object GetValueToAssign()
		{
			LinkField linkField = Field;

			if (linkField.IsInternal)
			{
				return linkField.TargetItem?.GetUrl();
			}

			return linkField.Url;
		}
	}
}
