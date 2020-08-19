namespace Constellation.Feature.Navigation.Models
{
	/// <summary>
	/// Represents a named group of Navigation Links. Can be further
	/// divided into sub-groups.
	/// </summary>
	public class LinkGroup : DeclaredNode
	{
		/// <summary>
		/// The name of the Link Group Sitecore Item
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The Display Name of the Link Group Sitecore Item
		/// </summary>
		public string DisplayName { get; set; }

		/// <summary>
		/// Indicates that one of the Child Links or Child Groups of this group is an ancestor of the Request's Context Item
		/// </summary>
		public bool IsRelevantToContext { get; set; }
	}
}
