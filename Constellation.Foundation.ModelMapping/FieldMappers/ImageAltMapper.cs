using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	/// <inheritdoc />
	/// <summary>
	/// Maps the "Alt" Attribute of an Image field to a Model Property suffixed with the word "Alt".
	/// </summary>
	public class ImageAltMapper : FieldAttributeMapper
	{
		/// <inheritdoc />
		protected override string GetPropertyName()
		{
			return base.GetPropertyName() + "Alt";
		}

		/// <inheritdoc />
		protected override object GetValueToAssign()
		{
			ImageField field = Field;

			return field.Alt;
		}
	}
}
