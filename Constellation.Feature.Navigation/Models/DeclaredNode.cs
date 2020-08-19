using System;
using System.Collections.Generic;
#pragma warning disable 618

namespace Constellation.Feature.Navigation.Models
{
	/// <summary>
	/// A member of a "static" navigation menu.
	/// </summary>
	public class DeclaredNode
	{
		/// <summary>
		/// Creates an instance of DeclaredNode
		/// </summary>
		public DeclaredNode()
		{
			Children = new List<DeclaredNode>();
			ChildLinks = new List<NavigationLink>();
			ChildGroups = new List<LinkGroup>();
		}

		/// <summary>
		/// Represents the Menu, Link Group or Navigation Link that manages this Node instance.
		/// </summary>
		public DeclaredNode Parent { get; set; }

		private bool _isActive;

		/// <summary>
		/// Indicates that one of the descendant links of this Node points to an Item that is an Ancestor to the
		/// Request's Context Item.
		/// </summary>
		public bool IsActive
		{
			get
			{
				return _isActive;
			}
			set
			{
				_isActive = value;

				if (Parent != null)
				{
					Parent.IsActive = _isActive;
				}
			}
		}

		/// <summary>
		/// The nodes immediately below this node in the Content Tree.
		/// </summary>
		public ICollection<DeclaredNode> Children { get; }

		/// <summary>
		/// Any Navigation Link (including descending types) Items stored as children of the Link Group Item.
		/// </summary>
		[Obsolete("Use DeclaredNode.Children instead. This property will be removed in a future version.")]
		public ICollection<NavigationLink> ChildLinks { get; }

		/// <summary>
		/// Any Link Group Items stored as children of the current Link Group Item.
		/// </summary>
		[Obsolete("Use DeclaredNode.Children instead. This property will be removed in a future version.")]
		public ICollection<LinkGroup> ChildGroups { get; }
	}
}
