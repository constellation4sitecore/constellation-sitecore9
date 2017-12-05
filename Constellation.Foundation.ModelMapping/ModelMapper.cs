using Constellation.Foundation.Data;
using Constellation.Foundation.ModelMapping.FieldMappers;
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

			// Map Item attributes such as Name, DisplayName, ID, and URL to model properties
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

			item.Fields.ReadAll();

			// Here's the interesting part where we map Item fields to model properties.
			foreach (Field field in item.Fields)
			{
				if (string.IsNullOrEmpty(field.Value))
				{
					continue; // no point in working to send a null value.
				}

				// Get field mappers
				var mappers = ModelMapperConfiguration.Current.GetMappersForFieldType(field.Type);

				foreach (Type mapperType in mappers)
				{
					try
					{
						var mapper = (IFieldMapper)Activator.CreateInstance(mapperType);

						var status = mapper.Map(model, field);

						switch (status)
						{
							case FieldMapStatus.Exception:
								Log.Error($"Mapping field {field.Name} on Item {item.Name}: Exception handled by field mapper.", typeof(ModelMapper));
								break;
							case FieldMapStatus.TypeMismatch:
								Log.Warn($"Mapping field {field.Name} on Item {item.Name} to Model {type.Name} failed.", typeof(ModelMapper));
								break;
							case FieldMapStatus.ExplicitIgnore:
								Log.Debug($"Mapping field {field.Name} on Item {item.Name}: explicitly ignored", typeof(ModelMapper));
								break;
							case FieldMapStatus.FieldEmpty:
								Log.Debug($"Mapping field {field.Name} on Item {item.Name}: field was empty.", typeof(ModelMapper));
								break;
							case FieldMapStatus.NoProperty:
								Log.Debug($"Mapping field {field.Name} on Item {item.Name}: no matching property name.", typeof(ModelMapper));
								break;
							case FieldMapStatus.ValueEmpty:
								Log.Debug($"Mapping field {field.Name} on Item {item.Name}: processed value was empty.", typeof(ModelMapper));
								break;
							case FieldMapStatus.Success:
								Log.Debug($"Mapping field {field.Name} on Item {item.Name}: success.", typeof(ModelMapper));
								break;
						}
					}
					catch (TypeLoadException ex)
					{
						Log.Error($"ModelMapper was unable to create FieldMapper type {mapperType.Name}", ex, typeof(ModelMapper));
					}
					catch (Exception ex)
					{
						Log.Error($"Mapping field {field.Name} on Item {item.Name} to Model {type.Name} failed.", ex, typeof(ModelMapper));
					}
				}
			}
		}
	}
}
