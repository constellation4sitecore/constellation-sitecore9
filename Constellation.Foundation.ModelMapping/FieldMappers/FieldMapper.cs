using System;
using System.Reflection;
using System.Web;
using Constellation.Foundation.Data;
using Constellation.Foundation.ModelMapping.MappingAttributes;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Web.UI.WebControls;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public abstract class FieldMapper<T> : IFieldMapper
	{
		#region Fields

		private string _propertyName = null;
		private PropertyInfo _property = null;

		#endregion

		#region Properties

		protected object Model { get; set; }

		protected Field Field { get; set; }

		protected virtual string PropertyName
		{
			get
			{
				if (_propertyName == null)
				{
					_propertyName = Field.Name.AsPropertyName();
				}

				return _propertyName;
			}
		}

		protected PropertyInfo Property
		{
			get
			{
				if (_property == null)
				{
					_property = Model.GetType().GetProperty(PropertyName, BindingFlags.Instance | BindingFlags.Public);
				}

				return _property;
			}
		}

		#endregion

		public virtual FieldMapStatus Map(object modelInstance, Field field)
		{
			Model = modelInstance;
			Field = field;

			if (Property == null)
			{
				return FieldMapStatus.NoProperty;
			}

			if (Property.GetCustomAttribute<DoNotMapAttribute>() != null)
			{
				return FieldMapStatus.ExplicitIgnore;
			}

			if (Property.GetCustomAttribute<RawValueOnlyAttribute>() != null)
			{
				if (Property.IsHtml())
				{
					Property.SetValue(Model, new HtmlString(Field.Value));
					return FieldMapStatus.Success;
				}

				Property.SetValue(Model, Field.Value);
				return FieldMapStatus.Success;
			}

			var paramsAttribute = Property.GetCustomAttribute<FieldRendererParamsAttribute>();

			if (paramsAttribute != null)
			{
				if (Property.IsHtml())
				{
					Property.SetValue(Model, new HtmlString(FieldRenderer.Render(Field.Item, Field.Name, paramsAttribute.Params)));
					return FieldMapStatus.Success;
				}

				Property.SetValue(Model, FieldRenderer.Render(Field.Item, Field.Name, paramsAttribute.Params));
				return FieldMapStatus.Success;
			}

			// Place to handle more complex scenarios.
			try
			{
				if (Property.IsHtml())
				{
					Property.SetValue(Model, new HtmlString(ExtractStringValueFromField()));
					return FieldMapStatus.Success;
				}

				if (Property.IsString())
				{
					Property.SetValue(Model, ExtractStringValueFromField());
					return FieldMapStatus.Success;
				}

				if (!PropertyTypeMatches())
				{
					return FieldMapStatus.TypeMismatch;
				}

				Property.SetValue(Model, ExtractTypedValueFromField());
				return FieldMapStatus.Success;
			}
			catch (Exception ex)
			{
				Log.Error($"Failed to convert Field {Field.Name} of Item {Field.Item.Name} to {typeof(T).Name}", ex, this);
				return FieldMapStatus.Exception;
			}
		}

		protected virtual string ExtractStringValueFromField()
		{
			return FieldRenderer.Render(Field.Item, Field.Name);
		}

		protected virtual bool PropertyTypeMatches()
		{
			return Property.Is<T>();
		}

		protected abstract T ExtractTypedValueFromField();
	}
}
