using Sitecore.Data.Fields;
using System.Reflection;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class DateFieldMapper : FieldMapper
	{
		public DateFieldMapper(object modelInstance, PropertyInfo property, Field field) : base(modelInstance, property, field)
		{

		}

		protected override object ExtractTypedValueFromField()
		{
			return ((DateField)Field).DateTime;
		}
	}
}
