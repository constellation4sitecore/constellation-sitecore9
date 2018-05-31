using System.Reflection;
using System.Web;
using Constellation.Foundation.Data;
using Constellation.Foundation.ModelMapping.MappingAttributes;
using Sitecore.Data.Fields;
using Sitecore.Web.UI.WebControls;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class GeneralLinkFieldMapper : IFieldMapper
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

		public FieldMapStatus Map(object modelInstance, Field field)
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

			var urlAttribute = Property.GetCustomAttribute<RenderAsUrlAttribute>();

			if (urlAttribute == null ||
				urlAttribute.UseFieldRendererInEditor && Sitecore.Context.PageMode.IsExperienceEditorEditing)
			{
				if (Property.IsHtml())
				{
					Property.SetValue(Model, new HtmlString(FieldRenderer.Render(Field.Item, Field.Name)));
					return FieldMapStatus.Success;
				}

				Property.SetValue(Model, FieldRenderer.Render(Field.Item, Field.Name));
				return FieldMapStatus.Success;
			}

			LinkField linkField = Field;

			Property.SetValue(Model, linkField.GetFriendlyUrl()); // Sitecore API has all the tricks for getting specific URL types out of this field.
			return FieldMapStatus.Success;
		}
	}
}
