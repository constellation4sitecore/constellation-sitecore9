using Constellation.Foundation.ModelMapping.MappingAttributes;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Web.UI.WebControls;
using System;
using System.Reflection;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public abstract class FieldMapper
	{
		protected FieldMapper(object modelInstance, PropertyInfo property, Field field)
		{
			Model = modelInstance;
			Field = field;
			Property = property;
		}

		#region Properties
		protected object Model { get; }

		protected Field Field { get; }

		protected PropertyInfo Property { get; }

		#endregion

		public virtual FieldMapStatus Map()
		{
			object value = null;

			if (Property.GetCustomAttribute<DoNotMapAttribute>() != null)
			{
				return FieldMapStatus.ExplicitIgnore;
			}

			if (Property.GetCustomAttribute<RawValueOnlyAttribute>() != null)
			{
				Property.SetValue(Model, Field.Value);
				return FieldMapStatus.Success;
			}

			var paramsAttribute = Property.GetCustomAttribute<FieldRendererParamsAttribute>();

			if (paramsAttribute != null)
			{
				Property.SetValue(Model, FieldRenderer.Render(Field.Item, Field.Name, paramsAttribute.Params));
				return FieldMapStatus.Success;
			}

			// Place to handle more complex scenarios.
			try
			{
				if (PropertyIsString())
				{
					Property.SetValue(Model, ExtractStringValueFromField());
					return FieldMapStatus.Success;
				}

				value = ExtractTypedValueFromField();

				if (!PropertyIsTypeMatch(value))
				{
					return FieldMapStatus.TypeMismatch;
				}
			}
			catch (Exception ex)
			{
				Log.Error("Exception generated while transforming an Item field for ModelMapping", ex, this);
				return FieldMapStatus.Exception;
			}

			if (value == null)
			{
				return FieldMapStatus.FieldEmpty;
			}

			Property.SetValue(Model, value);
			return FieldMapStatus.Success;
		}

		protected virtual string ExtractStringValueFromField()
		{
			return FieldRenderer.Render(Field.Item, Field.Name);
		}

		protected virtual object ExtractTypedValueFromField()
		{
			return Field.Value;
		}

		#region Protected Helpers

		protected bool PropertyIsString()
		{
			return PropertyIsTypeMatch(typeof(string));
		}

		protected bool PropertyIsTypeMatch(object candidateValue)
		{
			return candidateValue.GetType().IsAssignableFrom(Property.PropertyType);
		}
		#endregion

	}
}
