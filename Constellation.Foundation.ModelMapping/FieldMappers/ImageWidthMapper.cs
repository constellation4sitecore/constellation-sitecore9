using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class ImageWidthMapper : FieldAttributeMapper
	{
		protected override string GetPropertyName()
		{
			return base.GetPropertyName() + "Width";
		}

		protected override object GetValueToAssign()
		{
			ImageField field = Field;

			return field.Width;
		}
	}
}
