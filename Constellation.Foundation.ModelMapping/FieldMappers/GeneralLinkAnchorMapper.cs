using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	/// <inheritdoc />
	/// <summary>
	/// Maps the "Anchor" Attribute of a General Link field to a Model Property suffixed with the word "Anchor".
	/// </summary>
	public class GeneralLinkAnchorMapper : FieldAttributeMapper
	{
		/// <inheritdoc />
		protected override string GetPropertyName()
		{
			return base.GetPropertyName() + "Anchor";
		}

		/// <inheritdoc />
		protected override object GetValueToAssign()
		{
			LinkField field = Field;

			return field.Anchor;
		}
	}
}
