using System;
using System.Reflection;
using System.Web;
using Constellation.Foundation.Data;
using Constellation.Foundation.ModelMapping.MappingAttributes;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;
using Sitecore.Web.UI.WebControls;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	/// <inheritdoc />
	/// <summary>
	/// Given a Sitecore File Field, extract the Field's Value and assign it to the Model's Property.
	/// </summary>
	public class FileFieldMapper : IFieldMapper
	{
		#region Fields

		private string _propertyName = null;
		private PropertyInfo _property = null;

		#endregion

		#region Properties

		/// <summary>
		/// The Model to populate.
		/// </summary>
		protected object Model { get; set; }

		/// <summary>
		/// The Field to extract the value from.
		/// </summary>
		protected Field Field { get; set; }

		/// <summary>
		/// The expected name of the Property on the Model.
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
		/// The Property on the Model to fill with the Field Value.
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


		/// <inheritdoc />
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

				return FieldMapStatus.TypeMismatch;
			}
			catch (Exception ex)
			{
				Log.Error($"Failed to map Field {Field.Name} of Item {Field.Item.Name}", ex, this);
				return FieldMapStatus.Exception;
			}
		}

		/// <summary>
		/// Gets the value of the Field as a string. Default is to use FieldRenderer.Render()
		/// </summary>
		/// <returns>A string value to assign to the Property.</returns>
		protected string ExtractStringValueFromField()
		{
			var urlAttribute = Property.GetCustomAttribute<RenderAsUrlAttribute>();

			if (urlAttribute == null ||
				urlAttribute.UseFieldRendererInEditor && Sitecore.Context.PageMode.IsExperienceEditorEditing)
			{
				return FieldRenderer.Render(Field.Item, Field.Name);
			}

			return GetUrlFromField();
		}

		private string GetUrlFromField()
		{
			var targetFile = ((FileField)Field).MediaItem;

			if (targetFile != null)
			{
				return MediaManager.GetMediaUrl(targetFile);
			}

			return string.Empty;
		}
	}
}
