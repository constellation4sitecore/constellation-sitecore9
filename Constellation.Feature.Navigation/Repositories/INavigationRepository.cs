using Constellation.Feature.Navigation.Models;
using Sitecore.Data.Items;

namespace Constellation.Feature.Navigation.Repositories
{
	public interface INavigationRepository
	{
		NavigationMenu GetNavigation(Item datasource);

		NavigationNode GetSectionNavigation(Item contextItem, bool traverseFolders);
	}
}
