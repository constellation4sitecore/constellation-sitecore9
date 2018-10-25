using System;
using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	/// <inheritdoc />
	/// <summary>
	/// Maps the "TargetItem" Attribute of a General Link field to a Model Property suffixed with the word "TargetItem".
	/// </summary>
	public class GeneralLinkTargetItemMapper : FieldAttributeMapper
	{
		/// <inheritdoc />
		protected override string GetPropertyName()
		{
			return base.GetPropertyName() + "TargetItem";
		}

		/// <inheritdoc />
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
