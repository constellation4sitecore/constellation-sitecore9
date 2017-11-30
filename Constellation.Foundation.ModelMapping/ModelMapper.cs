using Constellation.Foundation.Data;
using Constellation.Foundation.ModelMapping.FieldMappers;
using Constellation.Foundation.Mvc;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System;
using System.Reflection;

namespace Constellation.Foundation.ModelMapping
{
	public static class ModelMapper
	{
		public static T MapItemToNew<T>(Item item)
			where T : new()
		{
			var model = new T();

			MapTo(item, model);

			return model;
		}

		public static void MapTo<T>(Item item, T model)
			where T : new()
		{
			var type = typeof(T);

			// Deal with Name, DisplayName, ID, and URL
			var nameProperty = type.GetProperty("Name", BindingFlags.Instance | BindingFlags.Public);

			if (nameProperty != null)
			{
				nameProperty.SetValue(model, item.Name);
			}

			var displayNameProperty = type.GetProperty("DisplayName", BindingFlags.Instance | BindingFlags.Public);

			if (displayNameProperty != null)
			{
				displayNameProperty.SetValue(model, item.DisplayName);
			}

			var idProperty = type.GetProperty("ID", BindingFlags.Instance | BindingFlags.Public);

			if (idProperty != null)
			{
				idProperty.SetValue(model, item.ID);
			}

			var urlProperty = type.GetProperty("Url", BindingFlags.Instance | BindingFlags.Public);

			if (urlProperty != null)
			{
				if (urlProperty.PropertyType == typeof(Uri))
				{
					urlProperty.SetValue(model, new Uri(item.GetUrl()));
				}
				else
				{
					urlProperty.SetValue(model, item.GetUrl());
				}
			}

			foreach (Field field in item.Fields)
			{
				if (string.IsNullOrEmpty(field.Value))
				{
					continue; // no point in working to send a null value.
				}

				var propertyName = field.Name.AsPropertyName();
				var property = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);

				if (property == null || !property.CanWrite)
				{
					continue; // Didn't find anything we can work with.
				}

				FieldMapStatus status;

				// For now we want to ignore list fields
				switch (field.Type)
				{
					case "Checkbox":
						status = new CheckboxFieldMapper(model, property, field).Map();
						break;
					case "Date":
					case "DateTime":
						status = new DateFieldMapper(model, property, field).Map();
						break;
					case "Number":
						status = new NumberFieldMapper(model, property, field).Map();
						break;
					case "Droplink":
					case "Droptree":
						status = new LinkFieldMapper(model, property, field).Map();
						break;
					case "File":
					case "Image":
						status = new MediaFieldMapper(model, property, field).Map();
						break;
					case "General Link":
					case "General Link with Search":
						status = new GeneralLinkFieldMapper(model, property, field).Map();
						break;
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
						status = new TextFieldMapper(model, property, field).Map();
						break;
				}

				if (status != FieldMapStatus.Success)
				{
					Log.Warn($"Mapping field {field.Name} on Item {item.Name} to Model {type.Name}.{property.Name} failed.", typeof(ModelMapper));
				}
			}
		}
	}
}
