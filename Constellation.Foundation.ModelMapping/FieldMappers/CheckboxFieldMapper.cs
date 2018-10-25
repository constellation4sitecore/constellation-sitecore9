using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	/// <summary>
	/// Maps a Sitecore Checkbox field to a boolean.
	/// </summary>
	public class CheckboxFieldMapper : FieldMapper<bool>
	{
		/// <summary>
		/// returns a boolean value given the Field.
		/// </summary>
		/// <returns>the status of CheckboxField.Checked.</returns>
		protected override bool ExtractTypedValueFromField()
		{
			return ((CheckboxField)Field).Checked;
		}
	}
}
