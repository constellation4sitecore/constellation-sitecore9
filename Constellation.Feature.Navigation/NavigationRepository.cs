using Constellation.Feature.Navigation.Models;
using Constellation.Foundation.Data;
using Constellation.Foundation.ModelMapping;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Constellation.Feature.Navigation
{
	/// <summary>
	/// Given a Datasource that represents a Navigation Menu, will generate
	/// a tree of ViewModels for use in constructing HTML page navigation.
	/// Useful for static navigation such as primary site menus or footers.
	/// </summary>
	public class NavigationRepository
	{
		/// <summary>
		/// Returns an instance of NavigationMenu with available link groups and navigation links
		/// pre-populated and ready with display-compatible values.
		/// </summary>
		/// <param name="datasource">The Navigation Menu Item to process.</param>
		/// <returns></returns>
		public static NavigationMenu GetNavigation(Item datasource)
		{
			var output = datasource.MapToNew<NavigationMenu>();

			ProcessGroupChildren(datasource, output);

			return output;
		}

		private static void ProcessGroupChildren(Item parent, LinkGroup parentGroup)
		{
			var children = parent.GetChildren();

			foreach (Item child in children)
			{
				if (child.IsDerivedFrom(NavigationTemplateIDs.LinkGroupID))
				{
					var group = child.MapToNew<LinkGroup>();
					parentGroup.ChildGroups.Add(group);
					ProcessGroupChildren(child, group);
					continue;
				}

				var link = GetLinkChild(child);
				if (link == null) continue;

				parentGroup.ChildLinks.Add(link);
				ProcessLinkChildren(child, link);
			}
		}

		private static void ProcessLinkChildren(Item parent, NavigationLink parentLink)
		{
			var children = parent.GetChildren();

			foreach (Item child in children)
			{
				var link = GetLinkChild(child);
				if (link == null) continue;
				parentLink.ChildLinks.Add(link);
				ProcessLinkChildren(child, link);
			}
		}

		private static NavigationLink GetLinkChild(Item child)
		{
			if (!child.IsDerivedFrom(NavigationTemplateIDs.NavigationLinkID))
			{
				LogIncompatibleItemWarning(child);
				return null;
			}
			if (child.IsDerivedFrom(NavigationTemplateIDs.ImageNavigationLinkID))
			{
				return child.MapToNew<ImageNavigationLink>();
			}

			return child.MapToNew<NavigationLink>();
		}

		private static void LogIncompatibleItemWarning(Item item)
		{
			Log.Warn(
				$"Navigation Repository encountered an Item in navigation that does not descend from Link Group or Navigation Link. Item {item.Paths.FullPath} ignored.",
				typeof(NavigationRepository));
		}
	}
}
