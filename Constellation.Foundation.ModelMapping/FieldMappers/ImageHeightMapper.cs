using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class ImageHeightMapper : FieldAttributeMapper
	{
		protected override string GetPropertyName()
		{
			return base.GetPropertyName() + "Height";
		}

		protected override object GetValueToAssign()
		{
			ImageField field = Field;

			return field.Height;
		}
	}
}
