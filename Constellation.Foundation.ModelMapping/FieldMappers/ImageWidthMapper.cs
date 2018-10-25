using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	/// <inheritdoc />
	/// <summary>
	/// Maps the "Width" Attribute of an Image field to a Model Property suffixed with the word "Width".
	/// </summary>
	public class ImageWidthMapper : FieldAttributeMapper
	{
		/// <inheritdoc />
		protected override string GetPropertyName()
		{
			return base.GetPropertyName() + "Width";
		}

		/// <inheritdoc />
		protected override object GetValueToAssign()
		{
			ImageField field = Field;

			return field.Width;
		}
	}
}
