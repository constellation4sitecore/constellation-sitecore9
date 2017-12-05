using Constellation.Foundation.Data;
using Constellation.Foundation.ModelMapping.MappingAttributes;
using Sitecore.Data.Fields;
using System;
using System.Reflection;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class InternalLinkFieldMapper : FieldMapper<Uri>
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

		protected override Uri ExtractTypedValueFromField()
		{
			return new Uri(((InternalLinkField)Field).TargetItem.GetUrl());
		}
	}
}
