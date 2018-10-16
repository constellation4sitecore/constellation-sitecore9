using System;
using System.Reflection;
using Constellation.Foundation.Data;
using Constellation.Foundation.ModelMapping.FieldMappers;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Constellation.Foundation.ModelMapping
{
	internal static class ModelBuilder
	{
		#region Methods
		internal static void MapItemToModel(Item item, object model)
		{
			Assert.ArgumentNotNull(item, "item");
			Assert.ArgumentNotNull(model, "model");

			MapItemProperties(item, model);

			var type = model.GetType();
			var plan = PlanCache.GetPlan(type.FullName, item.TemplateID);

			if (plan == null)
			{
				Log.Debug($"No plan for type {type.FullName} and template {item.TemplateID}. One will be recorded during the mapping.", typeof(ModelBuilder));
				MapItemFieldsAndCreatePlan(item, model);
			}
			else
			{
				Log.Debug($"Using plan for type {plan.TypeFullName} and template {plan.TemplateID} for mapping.",
					typeof(ModelBuilder));
				MapItemFieldsFromPlan(item, model, plan);
			}
		}
		#endregion

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

					MapItemToModel(parent, parentModel);

					parentProperty.SetValue(model, parentModel);
				}
			}
		}

		private static void MapItemFieldsAndCreatePlan(Item item, object model)
		{
			var type = model.GetType();
			var plan = new MappingPlan(type.FullName, item.TemplateID);

			item.Fields.ReadAll();

			// Here's the interesting part where we map Item fields to model properties.
			foreach (Field field in item.Fields)
			{
				if (ModelMapperConfiguration.Current.IgnoreStandardFields && field.Name.StartsWith("__"))
				{
					continue; // save some time.
				}

				// Get field mappers
				var mappers = ModelMapperConfiguration.Current.GetMappersForFieldType(field.Type);

				try
				{
					foreach (Type mapperType in mappers)
					{
						try
						{
							var mapper = (IFieldMapper)Activator.CreateInstance(mapperType);

							var status = mapper.Map(model, field);

							switch (status)
							{
								case FieldMapStatus.Exception:
									Log.Error($"Mapping field {field.Name} on Item {item.Name}: Exception handled by field mapper.", typeof(ModelBuilder));
									break;
								case FieldMapStatus.TypeMismatch:
									Log.Warn($"Mapping field {field.Name} on Item {item.Name} to Model {type.Name} failed.", typeof(ModelBuilder));
									break;
								case FieldMapStatus.ExplicitIgnore:
									Log.Debug($"Mapping field {field.Name} on Item {item.Name}: explicitly ignored", typeof(ModelBuilder));
									break;
								case FieldMapStatus.NoProperty:
									Log.Debug($"Mapping field {field.Name} on Item {item.Name}: no matching property name.", typeof(ModelBuilder));
									break;
								case FieldMapStatus.FieldEmpty:
									Log.Debug($"Mapping field {field.Name} on Item {item.Name}: field was empty.", typeof(ModelBuilder));
									plan.AddField(field.ID);
									break;
								case FieldMapStatus.ValueEmpty:
									Log.Debug($"Mapping field {field.Name} on Item {item.Name}: processed value was empty.", typeof(ModelBuilder));
									plan.AddField(field.ID);
									break;
								case FieldMapStatus.Success:
									Log.Debug($"Mapping field {field.Name} on Item {item.Name}: success.", typeof(ModelBuilder));
									plan.AddField(field.ID);
									break;
							}
						}
						catch (TypeLoadException ex)
						{
							Log.Error($"ModelMapper was unable to create FieldMapper type {mapperType.Name}", ex, typeof(ModelBuilder));
							throw;
						}
						catch (Exception ex)
						{
							Log.Error($"Mapping field {field.Name} on Item {item.Name} to Model {type.Name} failed.", ex, typeof(ModelBuilder));
							throw;
						}
					}

					PlanCache.AddPlan(plan);
				}
				catch (Exception)
				{
					Log.Warn($"ModelMapper did not cache the plan for {item.Name} because there were errors during the mapping process.", typeof(ModelBuilder));
				}
			}
		}

		private static void MapItemFieldsFromPlan(Item item, object model, MappingPlan plan)
		{
			var type = model.GetType();
			Log.Debug($"Mapping Item {item.Name} using a cached mapping plan for {type.FullName}", typeof(ModelBuilder));

			item.Fields.ReadAll();
			var fieldNames = plan.GetFieldIDs();

			// Here's the interesting part where we map Item fields to model properties.
			foreach (var fieldName in fieldNames)
			{
				var field = item.Fields[fieldName];

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
								Log.Error($"Mapping field {field.Name} on Item {item.Name}: Exception handled by field mapper.", typeof(ModelBuilder));
								break;
							case FieldMapStatus.TypeMismatch:
								Log.Warn($"Mapping field {field.Name} on Item {item.Name} to Model {type.Name} failed.", typeof(ModelBuilder));
								break;
							case FieldMapStatus.ExplicitIgnore:
								Log.Debug($"Mapping field {field.Name} on Item {item.Name}: explicitly ignored", typeof(ModelBuilder));
								break;
							case FieldMapStatus.NoProperty:
								Log.Debug($"Mapping field {field.Name} on Item {item.Name}: no matching property name.", typeof(ModelBuilder));
								break;
							case FieldMapStatus.FieldEmpty:
								Log.Debug($"Mapping field {field.Name} on Item {item.Name}: field was empty.", typeof(ModelBuilder));
								break;
							case FieldMapStatus.ValueEmpty:
								Log.Debug($"Mapping field {field.Name} on Item {item.Name}: processed value was empty.", typeof(ModelBuilder));
								break;
							case FieldMapStatus.Success:
								Log.Debug($"Mapping field {field.Name} on Item {item.Name}: success.", typeof(ModelBuilder));
								break;
						}
					}
					catch (TypeLoadException ex)
					{
						Log.Error($"ModelMapper was unable to create FieldMapper type {mapperType.Name}", ex, typeof(ModelBuilder));
					}
					catch (Exception ex)
					{
						Log.Error($"Mapping field {field.Name} on Item {item.Name} to Model {type.Name} failed.", ex, typeof(ModelBuilder));
					}
				}
			}
		}
	}
}
