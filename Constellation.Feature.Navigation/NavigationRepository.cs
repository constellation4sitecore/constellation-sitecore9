using Constellation.Feature.Navigation.Models;
using Constellation.Foundation.Data;
using Constellation.Foundation.ModelMapping;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System.Collections.Generic;

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

		/// <summary>
		/// Returns a tree of Navigation Nodes that can be used to make the typical margin-hosted
		/// expanding navigation found on C-shaped websites the world over. Runs from the nearest
		/// Landing Page ancestor to the children of the Context Item
		/// </summary>
		/// <param name="contextItem">The Context Item</param>
		/// <param name="traverseFolders">Specify whether to stop looking for landing pages at a folder or to continue past a folder to find the nearest landing page. Default is false.</param>
		/// <returns>A tree of NavigationNodes or null.</returns>
		public static NavigationNode GetSectionNavigation(Item contextItem, bool traverseFolders = false)
		{
			Assert.ArgumentNotNull(contextItem, "contextItem");

			var landing = GetNearestLandingPage(contextItem, traverseFolders);

			if (landing == null)
			{
				return null;
			}

			var model = landing.MapToNew<NavigationNode>();
			model.IsActive = true;

			model.Children = GetDescendants(landing, contextItem);


			return model;
		}

		private static ICollection<NavigationNode> GetDescendants(Item parent, Item context)
		{
			var nodes = new List<NavigationNode>();
			var childItems = parent.GetChildren();

			foreach (Item item in childItems)
			{
				if (!item.IsDerivedFrom(NavigationTemplateIDs.PageID))
				{
					continue;
				}

				// Add it to the list
				var node = item.MapToNew<NavigationNode>();

				nodes.Add(node);

				if (!item.Axes.IsAncestorOf(context))
				{
					continue;
				}

				// Unfold the item.
				node.IsActive = true;
				node.Children = GetDescendants(item, context);
			}

			return nodes;
		}

		private static Item GetNearestLandingPage(Item context, bool traverseFolders)
		{
			while (true)
			{
				if (context == null)
				{
					return null;
				}

				if (context.ID == NavigationTemplateIDs.SitecoreContentNodeID)
				{
					return null;
				}


				if (!context.IsDerivedFrom(NavigationTemplateIDs.PageID) && !traverseFolders)
				{
					return null;
				}

				if (context.IsDerivedFrom(NavigationTemplateIDs.LandingPageID))
				{
					return context;
				}

				context = context.Parent;
			}
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
