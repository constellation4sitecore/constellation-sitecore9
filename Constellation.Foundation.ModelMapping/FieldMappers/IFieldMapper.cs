using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	/// <summary>
	/// An object for mapping a Sitecore Item Field's Value to a Property on a given Model.
	/// </summary>
	public interface IFieldMapper
	{
		/// <summary>
		/// Maps the supplied Field value to the supplied Model's property.
		/// </summary>
		/// <param name="modelInstance">The model to inspect.</param>
		/// <param name="field">The field to inspect.</param>
		/// <returns>The status of the mapping attempt.</returns>
		FieldMapStatus Map(object modelInstance, Field field);
	}
}
