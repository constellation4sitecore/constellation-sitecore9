using Sitecore.Data.Fields;
using System;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class InternalLinkTargetItemMapper : FieldAttributeMapper
	{
		protected override string GetPropertyName()
		{
			return base.GetPropertyName() + "TargetItem";
		}

		protected override object GetValueToAssign()
		{
			InternalLinkField field = Field;

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
