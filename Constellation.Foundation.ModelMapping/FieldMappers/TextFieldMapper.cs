using Sitecore.Data.Fields;
using System.Reflection;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class TextFieldMapper : FieldMapper
	{
		public TextFieldMapper(object modelInstance, PropertyInfo property, Field field) : base(modelInstance, property, field)
		{
		}
	}
}
