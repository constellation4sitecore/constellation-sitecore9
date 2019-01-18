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
	/// <inheritdoc />
	/// <summary>
	/// The base class for mapping a Sitecore Item Field's Value to a Property on a given Model.
	/// </summary>
	/// <typeparam name="T">the Type of the Property on the Model that will receive the Field's value.</typeparam>
	public abstract class FieldMapper<T> : IFieldMapper
	{
		#region Fields

		private string _propertyName = null;
		private PropertyInfo _property = null;

		#endregion

		#region Properties

		/// <summary>
		/// The Model being initialized.
		/// </summary>
		protected object Model { get; set; }

		/// <summary>
		/// The Field being parsed.
		/// </summary>
		protected Field Field { get; set; }

		/// <summary>
		/// The Name of the Property on the Model that will receive the Field Value.
		/// </summary>
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

		/// <summary>
		/// The Property on the Modle that will receive the Field Value.
		/// </summary>
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

		/// <summary>
		/// Given the Field, Finds a matching Property of type "T" on the Model and moves the value from the Field to the
		/// discovered Property.
		/// </summary>
		/// <param name="modelInstance">The Model to inspect.</param>
		/// <param name="field">The Field to inspect.</param>
		/// <returns>A status indicating if the mapping was successful along with useful performance data.</returns>
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
					var stringValue = ExtractStringValueFromField();
					Property.SetValue(Model, new HtmlString(stringValue));

					if (string.IsNullOrEmpty(stringValue))
					{
						return FieldMapStatus.FieldEmpty;
					}
					return FieldMapStatus.Success;
				}

				if (Property.IsString())
				{
					var stringValue = ExtractStringValueFromField();
					Property.SetValue(Model, stringValue);

					if (string.IsNullOrEmpty(stringValue))
					{
						return FieldMapStatus.FieldEmpty;
					}
					return FieldMapStatus.Success;
				}

				if (!PropertyTypeMatches())
				{
					return FieldMapStatus.TypeMismatch;
				}

				var objectValue = ExtractTypedValueFromField();

				Property.SetValue(Model, objectValue);

				if (objectValue == null)
				{
					return FieldMapStatus.ValueEmpty;
				}

				return FieldMapStatus.Success;
			}
			catch (Exception ex)
			{
				Log.Error($"Failed to convert Field {Field.Name} of Item {Field.Item.Name} to {typeof(T).Name}", ex, this);
				return FieldMapStatus.Exception;
			}
		}

		/// <summary>
		/// Takes the value of the Field and returns it for insertion into the Model's Property.
		/// Override this if you cannot support FieldRenderer.Render as the default.
		/// </summary>
		/// <returns>The string to assign to the Property. Default is FieldRenderer.Render() output.</returns>
		protected virtual string ExtractStringValueFromField()
		{
			return FieldRenderer.Render(Field.Item, Field.Name);
		}

		/// <summary>
		/// Does the Type of the Model's Property match the type "T" of this Field Mapper's declaration?
		/// </summary>
		/// <returns>True if the Property can be assigned "T"</returns>
		protected virtual bool PropertyTypeMatches()
		{
			return Property.Is<T>();
		}

		/// <summary>
		/// Takes the Field value and converts it to "T" for assignment to the Property.
		/// </summary>
		/// <returns>The value of the Field as "T"</returns>
		protected abstract T ExtractTypedValueFromField();
	}
}
