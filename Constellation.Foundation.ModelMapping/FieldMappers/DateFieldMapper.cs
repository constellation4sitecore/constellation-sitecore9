using System;
using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	/// <summary>
	/// Maps the value of a Sitecore Date Field to a DateTime property.
	/// </summary>
	public class DateFieldMapper : FieldMapper<DateTime>
	{
		/// <summary>
		/// returns the value of the field as a DateTime.
		/// </summary>
		/// <returns>The value of the field as a DateTime.</returns>
		protected override DateTime ExtractTypedValueFromField()
		{
			return ((DateField)Field).DateTime;
		}
	}
}
