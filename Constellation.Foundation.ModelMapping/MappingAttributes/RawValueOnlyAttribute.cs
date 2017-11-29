using System;

namespace Constellation.Foundation.ModelMapping.MappingAttributes
{
	/// <inheritdoc />
	/// <summary>
	/// If a model property is marked with this attribute, the Field.Value will always be mapped
	/// to the property, and FieldRenderer will not be called, even in the Editing context.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class RawValueOnlyAttribute : Attribute
	{
	}
}