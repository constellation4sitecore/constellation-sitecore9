using Sitecore.Data.Fields;
using System;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class DateFieldMapper : FieldMapper<DateTime>
	{
		protected override DateTime ExtractTypedValueFromField()
		{
			return ((DateField)Field).DateTime;
		}
	}
}
