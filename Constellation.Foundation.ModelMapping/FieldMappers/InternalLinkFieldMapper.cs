using System;
using System.Reflection;
using Constellation.Foundation.Data;
using Constellation.Foundation.ModelMapping.MappingAttributes;
using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	/// <inheritdoc />
	/// <summary>
	/// Given a Sitecore Field that stores a single ID, Write the URL of the Target Item to the Model's Property, or load the Target Item and map it to the Model's Property.
	/// </summary>
	public class InternalLinkFieldMapper : FieldMapper<object>
	{
		/// <summary>
		/// The String to assign to the Model's Property. Typically not used for this field type.
		/// </summary>
		/// <returns>A string to assign to the Property.</returns>
		protected override string ExtractStringValueFromField()
		{
			var urlAttribute = Property.GetCustomAttribute<RenderAsUrlAttribute>();

			if (urlAttribute == null || urlAttribute.UseFieldRendererInEditor && Sitecore.Context.PageMode.IsExperienceEditorEditing)
			{
				return base.ExtractStringValueFromField();
			}

			return ((InternalLinkField)Field).TargetItem.GetUrl();
		}

		/// <summary>
		/// Takes the Field's Target Item and passes it to ModelMapper to map to a new instance of an Object determined by the Property's Type.
		/// </summary>
		/// <returns>An instance of the Property's Type.</returns>
		protected override object ExtractTypedValueFromField()
		{
			InternalLinkField field = Field;

			var item = field.TargetItem;

			if (item != null)
			{
				var targetModel = Activator.CreateInstance(Property.PropertyType);

				MappingContext.Current.MapTo(item, targetModel);

				return targetModel;
			}

			return null;
		}
	}
}
