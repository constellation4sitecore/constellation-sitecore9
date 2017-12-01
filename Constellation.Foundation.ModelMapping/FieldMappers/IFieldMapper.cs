using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public interface IFieldMapper
	{
		FieldMapStatus Map(object modelInstance, Field field);
	}
}
