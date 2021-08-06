using System;

namespace Constellation.Foundation.Labels
{
	/// <summary>
	/// 
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class LabelAttribute : Attribute
	{
		/// <summary>
		/// Creates a new instance of the LabelAttribute class.
		/// </summary>
		/// <param name="templateID">The Sitecore Template that is used for a particular Label Group.</param>
		public LabelAttribute(string templateID)
		{
			TemplateID = templateID;
		}

		/// <summary>
		/// Gets or sets the ID of the template item that can be mapped to the annotated class.
		/// </summary>
		public string TemplateID { get; }
	}
}
