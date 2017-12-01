using Sitecore.Data.Fields;
using System;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class IntegerFieldMapper : FieldMapper<int>
	{
		protected override int ExtractTypedValueFromField()
		{
			if (int.TryParse(Field.Value, out var result))
			{
				return result;
			}

			throw new Exception("Field Value could not be parsed to Integer.");
		}
	}
}
