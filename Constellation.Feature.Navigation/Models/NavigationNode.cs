using System.Collections.Generic;

namespace Constellation.Feature.Navigation.Models
{
	/// <summary>
	/// Represents a link to a page in a typical accordion or tree-based navigation scheme.
	/// </summary>
	public class NavigationNode : TargetItem
	{
		public NavigationNode()
		{
			Children = new List<NavigationNode>();
		}

		/// <summary>
		/// The children of this page, if this page is an ancestor-or-current
		/// of the Context Item.
		/// </summary>
		public ICollection<NavigationNode> Children { get; set; }

		/// <summary>
		/// Indicates that this instance is an ancestor-or-current of the Context Item.
		/// </summary>
		public bool IsActive { get; set; }
	}
}
