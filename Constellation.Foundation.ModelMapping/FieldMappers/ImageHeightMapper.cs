using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	/// <inheritdoc />
	/// <summary>
	/// Maps the "Height" Attribute of an Image field to a Model Property suffixed with the word "Height".
	/// </summary>
	public class ImageHeightMapper : FieldAttributeMapper
	{
		/// <inheritdoc />
		protected override string GetPropertyName()
		{
			return base.GetPropertyName() + "Height";
		}

		/// <inheritdoc />
		protected override object GetValueToAssign()
		{
			ImageField field = Field;

			return field.Height;
		}
	}
}
