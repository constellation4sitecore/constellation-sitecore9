using System;

namespace Constellation.Foundation.ModelMapping.MappingAttributes
{
	/// <inheritdoc />
	/// <summary>
	/// When a Model property is marked with this attribute, the string value of the property will
	/// be mapped to the best-match URL of the target field, with no surrounding markup. In the case
	/// of an Image or File field, the MediaUrl will be provided. In the case of a General Link, the
	/// FriendlyUrl will be provided. If UseFieldRendererInEditor is true, the full Field Renderer
	/// markup for the source field will be mapped when the PageMode is Editing.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class RenderAsUrlAttribute : Attribute
	{
		/// <inheritdoc />
		/// <summary>
		/// Creates a new instance of the RenderAsUrlAttribute
		/// </summary>
		/// <param name="useFieldRendererInEditor">Flag to specify if FieldRenderer should be called when in Edit mode. Default is false.</param>
		public RenderAsUrlAttribute(bool useFieldRendererInEditor = false)
		{
			UseFieldRendererInEditor = useFieldRendererInEditor;
		}

		/// <summary>
		/// If true, the FieldRenderer will be called when the current request is in PageMode Editing.
		/// </summary>
		public bool UseFieldRendererInEditor { get; set; }
	}
}
