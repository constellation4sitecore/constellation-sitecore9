using Constellation.Feature.Navigation.Models;
using Sitecore.Data.Items;

namespace Constellation.Feature.Navigation.Repositories
{
	/// <summary>
	/// Given a Datasource that represents a Navigation Menu, will generate
	/// a tree of ViewModels for use in constructing HTML page navigation.
	/// Useful for static navigation such as primary site menus or footers.
	/// </summary>
	public interface IDeclaredNavigationRepository
	{
		/// <summary>
		/// Returns an instance of NavigationMenu with available link groups and navigation links
		/// pre-populated and ready with display-compatible values.
		/// </summary>
		/// <param name="datasource">The Navigation Menu Item to process.</param>
		/// <param name="contextItem">Optional. The Item represented by the current HttpRequest.</param>
		/// <returns>A Navigation Menu object that can be used to generate a menu system.</returns>
		NavigationMenu GetNavigation(Item datasource, Item contextItem = null);
	}
}
