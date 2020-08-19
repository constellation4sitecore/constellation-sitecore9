using Constellation.Feature.Navigation.Models;
using Constellation.Foundation.Data;
using Constellation.Foundation.ModelMapping;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
#pragma warning disable 618

namespace Constellation.Feature.Navigation.Repositories
{
	/// <summary>
	/// Given a Datasource that represents a Navigation Menu, will generate
	/// a tree of ViewModels for use in constructing HTML page navigation.
	/// Useful for static navigation such as primary site menus or footers.
	/// </summary>
	public class DeclaredNavigationRepository : IDeclaredNavigationRepository
	{
		#region Constructor
		/// <summary>
		/// Creates a new instance of DeclaredNaviationRepository
		/// </summary>
		/// <param name="modelMapper">The instance of IModelMapper to use for mapping Item fields to model properties, usually provided by dependency injection.</param>
		public DeclaredNavigationRepository(IModelMapper modelMapper)
		{
			ModelMapper = modelMapper;
		}
		#endregion

		#region Properties
		/// <summary>
		/// The instance of IModelMapper to use for mapping Item fields to model properties.
		/// </summary>
		protected IModelMapper ModelMapper { get; }
		#endregion

		/// <summary>
		/// Returns an instance of NavigationMenu with available link groups and navigation links
		/// pre-populated and ready with display-compatible values.
		/// </summary>
		/// <param name="datasource">The Navigation Menu Item to process.</param>
		/// <param name="contextItem">Optional. The Item represented by the current HttpRequest.</param>
		/// <returns></returns>
		public NavigationMenu GetNavigation(Item datasource, Item contextItem = null)
		{
			var output = ModelMapper.MapItemToNew<NavigationMenu>(datasource);

			ProcessChildren(datasource, output, contextItem);

			return output;
		}


		private void ProcessChildren(Item parent, DeclaredNode parentNode, Item contextItem)
		{
			var children = parent.GetChildren();

			foreach (Item child in children)
			{
				if (child.IsDerivedFrom(NavigationTemplateIDs.ImageNavigationLinkID))
				{
					var imageLink = ModelMapper.MapItemToNew<ImageNavigationLink>(child);
					imageLink.Parent = parentNode;
					imageLink.IsActive = LinkTargetIsAncestorOfContext(child, contextItem);
					parentNode.Children.Add(imageLink);
					parentNode.ChildLinks.Add(imageLink);

					ProcessChildren(child, imageLink, contextItem);
					continue;
				}

				if (child.IsDerivedFrom(NavigationTemplateIDs.NavigationLinkID))
				{
					var link = ModelMapper.MapItemToNew<NavigationLink>(child);
					link.Parent = parentNode;
					link.IsActive = LinkTargetIsAncestorOfContext(child, contextItem);
					parentNode.Children.Add(link);
					parentNode.ChildLinks.Add(link);

					ProcessChildren(child, link, contextItem);
					continue;
				}

				if (child.IsDerivedFrom(NavigationTemplateIDs.LinkGroupID))
				{
					var group = ModelMapper.MapItemToNew<LinkGroup>(child);
					group.Parent = parentNode;
					parentNode.Children.Add(group);
					parentNode.ChildGroups.Add(group);

					ProcessChildren(child, group, contextItem);
					continue;
				}

				// If we get here and the Item hasn't been processed, it's an unknown Item type.
				Log.Warn($"Declared Navigation Repository could not process an unsupported Item type: {child.TemplateName} for Item: {child.Paths.FullPath}.", this);
			}
		}

		private static bool LinkTargetIsAncestorOfContext(Item linkItem, Item contextItem)
		{
			if (contextItem == null)
			{
				return false;
			}

			LinkField field = linkItem.Fields["Link"];

			if (!field.IsInternal)
			{
				return false;
			}

			if (field.TargetItem == null)
			{
				return false;
			}

			return field.TargetItem.Axes.IsAncestorOf(contextItem);
		}
	}
}
