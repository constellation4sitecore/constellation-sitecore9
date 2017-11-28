using System;

namespace Constellation.Foundation.ModelMapping.MappingAttributes
{
	/// <inheritdoc />
	/// <summary>
	/// Use this attribute to pass information to the FieldRenderer for the decorated property (ex: image scaling values).
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class FieldRendererParamsAttribute : Attribute
	{
		/// <inheritdoc />
		/// <summary>
		/// A new instance of the attribute
		/// </summary>
		/// <param name="fieldRendererParams">The parameters string to pass to the FieldRenderer for this property.</param>
		public FieldRendererParamsAttribute(string fieldRendererParams)
		{
			Params = fieldRendererParams;
		}

		/// <summary>
		/// The parameter string to pass to Field Renderer.
		/// </summary>
		public string Params { get; set; }
	}
}
