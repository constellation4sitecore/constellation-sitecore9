using Constellation.Foundation.ModelMapping.MappingAttributes;
using Sitecore.Data.Fields;
using Sitecore.Resources.Media;
using System;
using System.Reflection;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class MediaFieldMapper : FieldMapper
	{
		public MediaFieldMapper(object modelInstance, PropertyInfo property, Field field) : base(modelInstance, property, field)
		{
		}

		protected override string ExtractStringValueFromField()
		{
			var urlAttribute = Property.GetCustomAttribute<RenderAsUrlAttribute>();

			if (urlAttribute == null || urlAttribute.UseFieldRendererInEditor && Sitecore.Context.PageMode.IsExperienceEditorEditing)
			{
				return base.ExtractStringValueFromField();
			}

			return GetUrlFromField();
		}

		protected override object ExtractTypedValueFromField()
		{
			var url = GetUrlFromField();

			if (!string.IsNullOrEmpty(url))
			{
				return new Uri(GetUrlFromField());
			}

			return null;
		}

		private string GetUrlFromField()
		{
			if (Field.Type == "File")
			{
				var targetFile = ((FileField)Field).MediaItem;

				if (targetFile != null)
				{
					return MediaManager.GetMediaUrl(targetFile);
				}
			}

			if (Field.Type == "Image")
			{
				var targetImage = ((ImageField)Field).MediaItem;

				if (targetImage != null)
				{
					return MediaManager.GetMediaUrl(targetImage);
				}
			}

			return string.Empty;
		}
	}
}
