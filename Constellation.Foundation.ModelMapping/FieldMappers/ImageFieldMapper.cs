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
	public class ImageFieldMapper : IFieldMapper
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

		protected string ExtractStringValueFromField()
		{
			var urlAttribute = Property.GetCustomAttribute<RenderAsUrlAttribute>();

			if (urlAttribute == null || urlAttribute.UseFieldRendererInEditor && Sitecore.Context.PageMode.IsExperienceEditorEditing)
			{
				return FieldRenderer.Render(Field.Item, Field.Name);
			}

			return GetUrlFromField();
		}

		private string GetUrlFromField()
		{
			ImageField field = Field;
			var targetImage = field.MediaItem;

			if (targetImage != null)
			{

				var options = new MediaUrlOptions()
				{
					Language = targetImage.Language
				};

				int width;
				if (int.TryParse(field.Width, out width) && width > 0)
				{
					options.Width = width;
				}

				int height;
				if (int.TryParse(field.Height, out height) && height > 0)
				{
					options.Height = height;
				}

				var innerUrl = MediaManager.GetMediaUrl(targetImage, options);
				var url = HashingUtils.ProtectAssetUrl(innerUrl);

				return url;
			}

			return string.Empty;
		}
	}
}
