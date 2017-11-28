using System;

namespace Constellation.Foundation.ModelMapping.MappingAttributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public class RenderAsUrlAttribute : Attribute
	{
		public RenderAsUrlAttribute(bool useFieldRendererInEditor = false)
		{
			UseFieldRendererInEditor = useFieldRendererInEditor;
		}

		public bool UseFieldRendererInEditor { get; set; }
	}
}
