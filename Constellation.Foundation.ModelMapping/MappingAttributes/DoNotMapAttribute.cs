using System;

namespace Constellation.Foundation.ModelMapping.MappingAttributes
{
	/// <inheritdoc />
	/// <summary>
	/// If a Model property is marked with this attribute, the ModelMapper will not try to populate the property.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class DoNotMapAttribute : Attribute
	{
	}
}
