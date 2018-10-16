using System;
using Sitecore.Data.Fields;

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

				MappingContext.Current.MapTo(item, targetModel);

				return targetModel;
			}

			return null;
		}
	}
}
