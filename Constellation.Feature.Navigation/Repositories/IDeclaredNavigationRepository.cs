using Constellation.Feature.Navigation.Models;
using Sitecore.Data.Items;

namespace Constellation.Feature.Navigation.Repositories
{
	public interface IDeclaredNavigationRepository
	{
		NavigationMenu GetNavigation(Item datasource, Item contextItem = null);
	}
}
