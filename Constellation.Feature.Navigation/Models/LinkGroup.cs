using System.Collections.Generic;

namespace Constellation.Feature.Navigation.Models
{
	/// <summary>
	/// Represents a named group of Navigation Links. Can be further
	/// divided into sub-groups.
	/// </summary>
	public class LinkGroup : DeclaredNode
	{
		/// <summary>
		/// Creates a new instance of LinkGroup
		/// </summary>
		public LinkGroup()
		{
			ChildLinks = new List<NavigationLink>();
			ChildGroups = new List<LinkGroup>();
		}

		/// <summary>
		/// The name of the Link Group Sitecore Item
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The Display Name of the Link Group Sitecore Item
		/// </summary>
		public string DisplayName { get; set; }

		/// <summary>
		/// Any Navigation Link (including descending types) Items stored as children of the Link Group Item.
		/// </summary>
		public ICollection<NavigationLink> ChildLinks { get; set; }

		/// <summary>
		/// Any Link Group Items stored as children of the current Link Group Item.
		/// </summary>
		public ICollection<LinkGroup> ChildGroups { get; set; }

		/// <summary>
		/// Indicates that one of the Child Links or Child Groups of this group is an ancestor of the Request's Context Item
		/// </summary>
		public bool IsRelevantToContext { get; set; }
	}
}
