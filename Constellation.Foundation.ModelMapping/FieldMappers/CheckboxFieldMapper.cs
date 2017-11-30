using Sitecore.Data.Fields;
using System.Reflection;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class CheckboxFieldMapper : FieldMapper
	{
		public CheckboxFieldMapper(object modelInstance, PropertyInfo property, Field field) : base(modelInstance, property, field)
		{
		}

		protected override object ExtractTypedValueFromField()
		{
			return ((CheckboxField)Field).Checked;
		}
	}
}
