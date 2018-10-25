using System;
using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	/// <inheritdoc />
	/// <summary>
	/// Given a Sitecore Numeric Field, Extract the Value as an Integer and assign it to the Model's Property.
	/// </summary>
	public class IntegerFieldMapper : FieldMapper<int>
	{
		/// <inheritdoc />
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
