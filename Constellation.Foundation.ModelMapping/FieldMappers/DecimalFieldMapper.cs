using System;
using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	/// <inheritdoc />
	/// <summary>
	/// Given a Sitecore numeric field, extract the value as a decimal and assign it to the Model's Property.
	/// </summary>
	public class DecimalFieldMapper : FieldMapper<decimal>
	{
		/// <inheritdoc />
		protected override decimal ExtractTypedValueFromField()
		{
			if (decimal.TryParse(Field.Value, out var result))
			{
				return result;
			}

			throw new Exception("Field Value could not be parsed to Decimal.");
		}
	}
}
