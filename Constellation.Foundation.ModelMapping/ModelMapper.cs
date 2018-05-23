using System;
using System.Collections.Generic;
using System.Reflection;
using Constellation.Foundation.Data;
using Constellation.Foundation.ModelMapping.FieldMappers;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Constellation.Foundation.ModelMapping
{
	public static class ModelMapper
	{

		public static ICollection<T> MapToCollectionOf<T>(ICollection<Item> items)
			where T : class, new()
		{
			var list = new List<T>();

			if (items != null)
			{
				foreach (var item in items)
				{
					list.Add(MapItemToNew<T>(item));
				}
			}

			return list;
		}

		public static IEnumerable<T> MapToEnumerableOf<T>(IEnumerable<Item> items)
			where T : class, new()
		{
			var list = new List<T>();

			if (items != null)
			{
				foreach (var item in items)
				{
					list.Add(MapItemToNew<T>(item));
				}
			}

			return list;
		}

		public static T MapItemToNew<T>(Item item)
			where T : class, new()
		{
			if (item == null)
			{
				return null;
			}

			var model = new T();

			MapTo(item, model);

			return model;
		}

		public static void MapTo(Item item, object model)
		{
			Assert.ArgumentNotNull(item, "item");

			MapItemProperties(item, model);
			MapItemFields(item, model);
		}

		private static void MapItemProperties(Item item, object model)
		{
			var type = model.GetType();

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
				urlProperty.SetValue(model, item.GetUrl());
			}

			var parentProperty = type.GetProperty("Parent", BindingFlags.Instance | BindingFlags.Public);

			if (parentProperty != null)
			{
				var parent = item.Parent;

				if (parent != null)
				{
					var parentModel = Activator.CreateInstance(parentProperty.PropertyType);

					MapTo(parent, parentModel);

					parentProperty.SetValue(model, parentModel);
				}
			}
		}

		private static void MapItemFields(Item item, object model)
		{
			var type = model.GetType();

			item.Fields.ReadAll();

			// Here's the interesting part where we map Item fields to model properties.
			foreach (Field field in item.Fields)
			{
				if (ModelMapperConfiguration.Current.IgnoreStandardFields && field.Name.StartsWith("__"))
				{
					continue; // save some time.
				}

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
