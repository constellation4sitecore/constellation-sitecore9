using Constellation.Feature.Navigation.Models;
using Sitecore.Data.Items;

namespace Constellation.Feature.Navigation.Repositories
{
	public interface IBranchNavigationRepository
	{
		BranchNode GetNavigation(Item contextItem, bool traverseFolders = false);
	}
}
