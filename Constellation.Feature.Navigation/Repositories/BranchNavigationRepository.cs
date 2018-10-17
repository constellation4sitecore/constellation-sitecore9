using System.Collections.Generic;
using Constellation.Feature.Navigation.Models;
using Constellation.Foundation.Data;
using Constellation.Foundation.ModelMapping;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Constellation.Feature.Navigation.Repositories
{
	public class BranchNavigationRepository : IBranchNavigationRepository
	{
		#region Constructor
		public BranchNavigationRepository(IModelMapper modelMapper)
		{
			ModelMapper = modelMapper;
		}
		#endregion

		#region Properties
		protected IModelMapper ModelMapper { get; }
		#endregion

		/// <summary>
		/// Returns a tree of Navigation Nodes that can be used to make the typical margin-hosted
		/// expanding navigation found on C-shaped websites the world over. Runs from the nearest
		/// Landing Page ancestor to the children of the Context Item
		/// </summary>
		/// <param name="contextItem">The Context Item</param>
		/// <param name="traverseFolders">Specify whether to stop looking for landing pages at a folder or to continue past a folder to find the nearest landing page. Default is false.</param>
		/// <returns>A tree of NavigationNodes or null.</returns>
		public BranchNode GetNavigation(Item contextItem, bool traverseFolders = false)
		{
			Assert.ArgumentNotNull(contextItem, "contextItem");

			var landing = GetNearestLandingPage(contextItem, traverseFolders);

			if (landing == null)
			{
				return null;
			}

			var model = ModelMapper.MapItemToNew<BranchNode>(landing);
			model.IsActive = true;

			model.Children = GetDescendants(landing, contextItem);

			return model;
		}

		private ICollection<BranchNode> GetDescendants(Item parent, Item context)
		{
			var nodes = new List<BranchNode>();
			var childItems = parent.GetChildren();

			foreach (Item item in childItems)
			{
				if (!item.IsDerivedFrom(NavigationTemplateIDs.PageID))
				{
					continue;
				}

				// Add it to the list
				var node = ModelMapper.MapItemToNew<BranchNode>(item);

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
	}
}
