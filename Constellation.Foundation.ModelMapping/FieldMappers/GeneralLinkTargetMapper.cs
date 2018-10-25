using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	/// <inheritdoc />
	/// <summary>
	/// Maps the "Target" Attribute of a General Link field to a Model Property suffixed with the word "Target".
	/// </summary>
	public class GeneralLinkTargetMapper : FieldAttributeMapper
	{
		/// <inheritdoc />
		protected override string GetPropertyName()
		{
			return base.GetPropertyName() + "Target";
		}

		/// <inheritdoc />
		protected override object GetValueToAssign()
		{
			LinkField field = Field;

			return field.Target;
		}
	}
}
