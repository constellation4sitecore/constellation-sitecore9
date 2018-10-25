using Constellation.Foundation.Data;
using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	/// <inheritdoc />
	/// <summary>
	/// Maps the "Url" Attribute of a General Link field to a Model Property suffixed with the word "Url".
	/// </summary>
	public class GeneralLinkUrlMapper : FieldAttributeMapper
	{
		/// <inheritdoc />
		protected override string GetPropertyName()
		{
			return base.GetPropertyName() + "Url";
		}

		/// <inheritdoc />
		protected override object GetValueToAssign()
		{
			LinkField linkField = Field;

			if (linkField.IsInternal)
			{
				return linkField.TargetItem?.GetUrl();
			}

			return linkField.Url;
		}
	}
}
