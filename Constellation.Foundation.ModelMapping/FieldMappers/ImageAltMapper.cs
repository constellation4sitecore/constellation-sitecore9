using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class ImageAltMapper : FieldAttributeMapper
	{
		protected override string GetPropertyName()
		{
			return base.GetPropertyName() + "Alt";
		}

		protected override object GetValueToAssign()
		{
			ImageField field = Field;

			return field.Alt;
		}
	}
}
