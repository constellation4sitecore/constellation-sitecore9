using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	/// <inheritdoc />
	/// <summary>
	/// Maps the "Class" Attribute of a General Link field to a Model Property suffixed with the word "Class".
	/// </summary>
	public class GeneralLinkClassMapper : FieldAttributeMapper
	{
		/// <inheritdoc />
		protected override string GetPropertyName()
		{
			return base.GetPropertyName() + "Class";
		}

		/// <inheritdoc />
		protected override object GetValueToAssign()
		{
			LinkField field = Field;

			return field.Class;
		}
	}
}
