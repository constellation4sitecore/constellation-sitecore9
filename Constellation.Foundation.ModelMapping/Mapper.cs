using Constellation.Foundation.Mvc;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
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

			// TODO - iterate through Properties looking for Matching Field Names
			// TODO - if the property specifies a field name via attribute, use that.
			// TODO - Otherwise, parse the property name to get the field name.

			// TODO - no field name, is the last word a property of the field object?

			var model = new T();

			foreach (Field field in item.Fields)
			{
				if (string.IsNullOrEmpty(field.Value))
				{
					continue;
				}
				var property = type.GetProperty(field.Name.AsPropertyName(), BindingFlags.Default);

				if (property == null || !property.CanWrite)
				{
					continue;
				}

				// Deal with Properties that have processing attributes

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

				var fieldType = field.Value.GetType();

				if (!property.PropertyType.IsAssignableFrom(fieldType))
				{
					continue;
				}

				property.SetValue(model, field.Value);
			}

			return new T();
		}
	}
}
