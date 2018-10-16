using System;
using System.Reflection;
using Constellation.Foundation.Data;
using Constellation.Foundation.ModelMapping.MappingAttributes;
using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class InternalLinkFieldMapper : FieldMapper<object>
	{
		protected override string ExtractStringValueFromField()
		{
			var urlAttribute = Property.GetCustomAttribute<RenderAsUrlAttribute>();

			if (urlAttribute == null || urlAttribute.UseFieldRendererInEditor && Sitecore.Context.PageMode.IsExperienceEditorEditing)
			{
				return base.ExtractStringValueFromField();
			}

			return ((InternalLinkField)Field).TargetItem.GetUrl();
		}

		protected override object ExtractTypedValueFromField()
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
