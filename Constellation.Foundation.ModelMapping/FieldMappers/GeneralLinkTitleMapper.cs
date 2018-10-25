using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	/// <inheritdoc />
	/// <summary>
	/// Maps the "Title" Attribute of a General Link field to a Model Property suffixed with the word "Title".
	/// </summary>
	public class GeneralLinkTitleMapper : FieldAttributeMapper
	{
		/// <inheritdoc />
		protected override string GetPropertyName()
		{
			return base.GetPropertyName() + "Title";
		}

		/// <inheritdoc />
		protected override object GetValueToAssign()
		{
			LinkField field = Field;

			return field.Title;
		}
	}
}
