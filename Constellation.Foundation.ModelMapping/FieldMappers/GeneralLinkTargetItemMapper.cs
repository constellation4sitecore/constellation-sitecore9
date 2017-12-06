using Sitecore.Data.Fields;
using System;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class GeneralLinkTargetItemMapper : FieldAttributeMapper
	{
		protected override string GetPropertyName()
		{
			return base.GetPropertyName() + "TargetItem";
		}

		protected override object GetValueToAssign()
		{
			LinkField field = Field;

			var item = field.TargetItem;

			if (item != null)
			{
				var targetModel = Activator.CreateInstance(Property.PropertyType);

				ModelMapper.MapTo(item, targetModel);

				return targetModel;
			}

			return null;
		}
	}
}
