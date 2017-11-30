using Constellation.Foundation.ModelMapping.MappingAttributes;
using Sitecore.Data.Fields;
using System;
using System.Reflection;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class GeneralLinkFieldMapper : FieldMapper
	{
		public GeneralLinkFieldMapper(object modelInstance, PropertyInfo property, Field field) : base(modelInstance, property, field)
		{

		}

		protected override string ExtractStringValueFromField()
		{
			var urlAttribute = Property.GetCustomAttribute<RenderAsUrlAttribute>();

			if (urlAttribute == null || urlAttribute.UseFieldRendererInEditor && Sitecore.Context.PageMode.IsExperienceEditorEditing)
			{
				return base.ExtractStringValueFromField();
			}

			return ((LinkField)Field).GetFriendlyUrl();
		}

		protected override object ExtractTypedValueFromField()
		{
			return new Uri(((LinkField)Field).GetFriendlyUrl());
		}
	}
}
