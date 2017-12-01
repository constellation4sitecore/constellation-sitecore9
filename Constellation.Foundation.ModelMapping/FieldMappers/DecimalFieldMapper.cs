using Sitecore.Data.Fields;
using System;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class DecimalFieldMapper : FieldMapper<decimal>
	{
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
