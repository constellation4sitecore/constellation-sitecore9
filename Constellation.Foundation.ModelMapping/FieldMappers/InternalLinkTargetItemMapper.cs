using System;
using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	/// <inheritdoc />
	/// <summary>
	/// Maps the "TargetItem" Attribute of an Internal Link field to a Model Property suffixed with the word "TargetItem".
	/// </summary>
	public class InternalLinkTargetItemMapper : FieldAttributeMapper
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override string GetPropertyName()
		{
			return base.GetPropertyName() + "TargetItem";
		}

		/// <inheritdoc />
		protected override object GetValueToAssign()
		{
			InternalLinkField field = Field;

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
