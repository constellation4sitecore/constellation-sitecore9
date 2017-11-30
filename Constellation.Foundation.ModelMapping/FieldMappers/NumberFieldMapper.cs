using Sitecore.Data.Fields;
using System;
using System.Reflection;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class NumberFieldMapper : FieldMapper
	{
		public NumberFieldMapper(object modelInstance, PropertyInfo property, Field field) : base(modelInstance, property, field)
		{
		}

		protected override object ExtractTypedValueFromField()
		{
			if (int.TryParse(Field.Value, out var result))
			{
				return result;
			}

			throw new Exception("Field Value could not be parsed to Integer.");
		}
	}
}
