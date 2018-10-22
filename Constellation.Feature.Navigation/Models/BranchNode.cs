using System.Collections.Generic;

namespace Constellation.Feature.Navigation.Models
{
	/// <summary>
	/// Represents a link to a page in a typical accordion or tree-based navigation scheme.
	/// </summary>
	public class BranchNode : TargetItem
	{
		/// <summary>
		/// Creats a new instance of BranchNode
		/// </summary>
		public BranchNode()
		{
			Children = new List<BranchNode>();
		}

		/// <summary>
		/// The children of this page, if this page is an ancestor-or-current
		/// of the Context Item.
		/// </summary>
		public ICollection<BranchNode> Children { get; set; }

		/// <summary>
		/// Indicates that this instance is an ancestor-or-current of the Context Item.
		/// </summary>
		public bool IsActive { get; set; }
	}
}
