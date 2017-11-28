using Constellation.Foundation.ModelMapping.MappingAttributes;
using Constellation.Foundation.Mvc;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Resources.Media;
using Sitecore.Web.UI.WebControls;
using System;
using System.Reflection;

namespace Constellation.Foundation.ModelMapping
{
	public class Mapper
	{
		public T Map<T>(Item item)
			where T : new()
		{
			var type = typeof(T);

			var model = new T();

			foreach (Field field in item.Fields)
			{
				if (string.IsNullOrEmpty(field.Value))
				{
					continue; // no point in working to send a null value.
				}

				// For now we want to ignore list fields
				switch (field.Type)
				{
					case "Checklist":
					case "Droplist":
					case "Grouped Droplink":
					case "Grouped Droplist":
					case "Multilist":
					case "Multilist with Search":
					case "Name Lookup Value List":
					case "Name Value List":
					case "Treelist":
					case "TreelistEx":
						continue;
					default:
						break;
				}

				var propertyName = field.Name.AsPropertyName();
				var property = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);

				if (property == null || !property.CanWrite)
				{
					continue;
				}

				var paramsAttribute = property.GetCustomAttribute<FieldRendererParamsAttribute>();

				if (paramsAttribute != null)
				{
					property.SetValue(model, FieldRenderer.Render(item, field.Name, paramsAttribute.Params));
					continue;
				}

				var urlAttribute = property.GetCustomAttribute<RenderAsUrlAttribute>();

				if (urlAttribute != null || property.PropertyType == typeof(Uri))
				{
					if (urlAttribute != null && urlAttribute.UseFieldRendererInEditor && Sitecore.Context.PageMode.IsExperienceEditorEditing)
					{
						property.SetValue(model, FieldRenderer.Render(item, field.Name));
						continue;
					}

					string url = string.Empty;


					// Droplink, Droptree
					if (field.Type == "Droplink" || field.Type == "Droptree")
					{
						LinkField linkField = field;
						var targetItem = linkField.TargetItem;

						if (targetItem != null)
						{
							url = LinkManager.GetItemUrl(targetItem);
						}
					}

					// Image, file
					if (field.Type == "Image")
					{
						ImageField imageField = field;
						var targetImage = imageField.MediaItem;

						if (targetImage != null)
						{
							var options = MediaUrlOptions.Empty;

							if (int.TryParse(imageField.Height, out var height)) options.Height = height;
							if (int.TryParse(imageField.Width, out var width)) options.Width = width;

							url = MediaManager.GetMediaUrl(targetImage, options);
						}
					}

					if (field.Type == "File")
					{
						var targetFile = ((FileField)field).MediaItem;

						if (targetFile != null)
						{
							url = MediaManager.GetMediaUrl(targetFile);
						}
					}

					// General Link
					if (field.Type.StartsWith("General Link"))
					{
						LinkField linkField = field;

						url = linkField.GetFriendlyUrl();
					}

					if (urlAttribute != null)
					{
						property.SetValue(model, url);
					}
					else
					{
						property.SetValue(model, new Uri(url));

					}

					continue;
				}

				if (property.PropertyType == typeof(string))
				{
					property.SetValue(model, FieldRenderer.Render(item, field.Name));
					continue;
				}

				if (property.PropertyType == typeof(int) && int.TryParse(field.Value, out var result))
				{
					property.SetValue(model, result);
					continue;
				}

				if (property.PropertyType == typeof(DateTime))
				{
					DateField dateField = field;

					property.SetValue(model, dateField.DateTime);
					continue;
				}

				property.SetValue(model, field.Value);
			}

			return model;
		}
	}
}
