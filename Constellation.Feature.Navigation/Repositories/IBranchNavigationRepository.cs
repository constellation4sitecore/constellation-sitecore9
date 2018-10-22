using Constellation.Feature.Navigation.Models;
using Sitecore.Data.Items;

namespace Constellation.Feature.Navigation.Repositories
{
	/// <summary>
	/// Generates a tree of Navigation Nodes that can be used to make the typical margin-hosted
	/// expanding navigation found on C-shaped websites the world over. Runs from the nearest
	/// Landing Page ancestor to the children of the Context Item
	/// </summary>
	public interface IBranchNavigationRepository
	{
		/// <summary>
		/// Returns a tree of Navigation Nodes that can be used to make the typical margin-hosted
		/// expanding navigation found on C-shaped websites the world over. Runs from the nearest
		/// Landing Page ancestor to the children of the Context Item
		/// </summary>
		/// <param name="contextItem">The Context Item</param>
		/// <param name="traverseFolders">Specify whether to stop looking for landing pages at a folder or to continue past a folder to find the nearest landing page. Default is false.</param>
		/// <returns>A tree of NavigationNodes or null.</returns>
		BranchNode GetNavigation(Item contextItem, bool traverseFolders = false);
	}
}
