using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using System.Collections.Generic;
using System.Globalization;

namespace Constellation.Foundation.Data
{
	/// <summary>
	/// Set of extensions for interacting with Sitecore items.
	/// </summary>
	public static class ItemExtensions
	{
		/// <summary>
		/// Work up the tree until an item is found that uses the specified template.
		/// </summary>
		/// <param name="current">The current item.</param>
		/// <param name="templateId">Template Id of the ancestor to look for.</param>
		/// <returns>The matching ancestor or null.</returns>
		public static Item FindAncestorByTemplateId(this Item current, string templateId)
		{
			return current.FindAncestorByTemplateId(new ID(templateId));
		}

		/// <summary>
		/// Work up the tree until an item is found that uses the specified template.
		/// </summary>
		/// <param name="current">The current item.</param>
		/// <param name="templateId">Template Id of the ancestor to look for.</param>
		/// <returns>The matching ancestor or null.</returns>
		public static Item FindAncestorByTemplateId(this Item current, ID templateId)
		{
			while (true)
			{
				if (current == null)
				{
					return null;
				}

				if (current.IsDerivedFrom(templateId))
				{
					return current;
				}

				current = current.Parent;
			}
		}

		/// <summary>
		/// Work up the tree until an item is found that uses the specified template.
		/// </summary>
		/// <param name="current">The current item.</param>
		/// <param name="baseTemplateId">Base template Id of the ancestor to look for.</param>
		/// <returns>The matching ancestor or null.</returns>
		public static Item FindAncestorByBaseTemplateId(this Item current, string baseTemplateId)
		{
			return current.FindAncestorByTemplateId(baseTemplateId);
		}

		/// <summary>
		/// Work up the tree until an item is found that uses the specified template.
		/// </summary>
		/// <param name="current">The current item.</param>
		/// <param name="baseTemplateId">Base template Id of the ancestor to look for.</param>
		/// <returns>The matching ancestor or null.</returns>
		public static Item FindAncestorByBaseTemplateId(this Item current, ID baseTemplateId)
		{
			return current.FindAncestorByTemplateId(baseTemplateId);
		}

		/// <summary>
		/// Work up the tree to see if there is an ancestor that matches the provided template id.
		/// If so get its Sitecore ID.
		/// </summary>
		/// <param name="current">The current item.</param>
		/// <param name="templateId">Template Id of the ancestor to look for.</param>
		/// <returns>ID of the matching ancestor or an empty string.</returns>
		public static string GetAncestorId(this Item current, string templateId)
		{
			Item ancestor = current?.FindAncestorByTemplateId(templateId);

			return ancestor?.ID.ToString() ?? string.Empty;
		}

		/// <summary>
		/// Gets the friendly URL of the item.
		/// </summary>
		/// <param name="item">The context item.</param>
		/// <returns>The url of the current item.</returns>
		public static string GetUrl(this Item item)
		{
			if (item != null)
			{
				var urlOptions = (UrlOptions)UrlOptions.DefaultOptions.Clone();

				urlOptions.SiteResolving = Sitecore.Configuration.Settings.Rendering.SiteResolving;

				return LinkManager.GetItemUrl(item, urlOptions);
			}

			return string.Empty;
		}

		/// <summary>
		/// Gets the friendly URL of the item.
		/// </summary>
		/// <param name="item">The context item.</param>
		/// <returns>The url of the current item.</returns>
		public static string GetUrl(this CustomItemBase item)
		{
			if (item?.InnerItem != null)
			{
				return GetUrl(item.InnerItem);
			}

			return string.Empty;
		}

		/// <summary>
		/// Determine if a particular item is derived from a specific template.
		/// </summary>
		/// <param name="item">The context item.</param>
		/// <param name="templateName">Template to check.</param>
		/// <returns>True if template matches, false if no match or either parameter is null.</returns>
		public static bool IsDerivedFrom(this Item item, string templateName)
		{
			if (string.IsNullOrEmpty(templateName))
			{
				return false;
			}

			var templateItem = item.Database.Templates[templateName];
			return item.IsDerivedFrom(templateItem);
		}

		/// <summary>
		/// Determine if a particular item is derived from a specific template.
		/// </summary>
		/// <param name="item">The context item.</param>
		/// <param name="templateId">Template to check.</param>
		/// <returns>True if template matches, false if no match or either parameter is null.</returns>
		public static bool IsDerivedFrom(this Item item, ID templateId)
		{
			if (templateId.IsNull)
			{
				return false;
			}

			var templateItem = item.Database.Templates[templateId];
			return item.IsDerivedFrom(templateItem);
		}

		/// <summary>
		/// Determine if a particular item is derived from a specific template.
		/// </summary>
		/// <param name="item">The context item.</param>
		/// <param name="templateToCompare">Template to check.</param>
		/// <returns>True if template matches, false if no match or either parameter is null.</returns>
		public static bool IsDerivedFrom(this Item item, TemplateItem templateToCompare)
		{
			if (item == null)
			{
				return false;
			}

			if (templateToCompare == null)
			{
				return false;
			}

			if (item.TemplateID == templateToCompare.ID)
			{
				return true;
			}

			var itemTemplate = item.Database.Templates[item.TemplateID];

			if (itemTemplate == null)
			{
				return false;
			}

			return itemTemplate.ID == templateToCompare.ID || TemplateIsDerivedFrom(itemTemplate, templateToCompare);
		}

		/// <summary>
		/// Check to see if the current item is a template.
		/// </summary>
		/// <param name="item">The context item.</param>
		/// <returns>True if the item is a template, false if not a template or item is null.</returns>
		public static bool IsTemplate(this Item item)
		{
			return item.Database.Engines.TemplateEngine.IsTemplatePart(item);
		}

		/// <summary>
		/// Get all child items that are derived from a particular template.
		/// </summary>
		/// <param name="item">The context item.</param>
		/// <param name="templateId">Template Id of desired items.</param>
		/// <returns>All children that have the supplied template.</returns>
		public static IEnumerable<Item> ChildrenDerivedFrom(this Item item, ID templateId)
		{
			var children = item.GetChildren();
			var childrenDerivedFrom = new List<Item>();

			foreach (Item child in children)
			{
				if (child.IsDerivedFrom(templateId))
				{
					childrenDerivedFrom.Add(child);
				}
			}

			return childrenDerivedFrom;
		}

		/// <summary>
		/// Determines if the current item is in the context site.
		/// </summary>
		/// <param name="context">The context item.</param>
		/// <returns>True if item is in the current site, false if not or item is null.</returns>
		public static bool IsInContextSite(this Item context)
		{
			if (context == null)
			{
				return false;
			}

			Assert.IsNotNull(Sitecore.Context.Site, "Sitecore.Context.Site required by the Item.IsInContextSite extension is null");
			Assert.IsNotNullOrEmpty(Sitecore.Context.Site.RootPath, "Sitecore.Context.Site.RootPath required by the Item.IsInSite extension is null or empty");
			Assert.IsNotNull(Sitecore.Context.Database, "Sitecore.Context.Database required by the Item.IsInSite extension is null");

			var rootItem = Sitecore.Context.Site.Database.Items[Sitecore.Context.Site.RootPath];
			Assert.IsNotNull(rootItem, string.Format(CultureInfo.CurrentCulture, "Unable to retrieve the item at path {0} using the database {1}", Sitecore.Context.Site.RootPath, Sitecore.Context.Database.Name));

			return rootItem.Axes.IsAncestorOf(context);
		}

		/// <summary>
		/// If a child item does not exist, create it.
		/// </summary>
		/// <param name="parent">The parent item.</param>
		/// <param name="name">The name of the item to find or create.</param>
		/// <param name="templateId">The template to use if creating a new item.</param>
		/// <returns>An item matching the name specified under the parent item.</returns>
		public static Item FindOrCreateChildItem(this Item parent, string name, ID templateId)
		{
			Assert.IsNotNull(parent, "parent cannot be null.");
			Assert.IsNotNullOrEmpty(name, "name cannot be null or empty.");
			Assert.IsNotNull(templateId, "templateId cannot be null.");

			var child = parent.Database.GetItem(parent.Paths.FullPath + "/" + name);

			if (child == null)
			{
				name = ItemUtil.ProposeValidItemName(name);

				child = parent.Add(name, new TemplateID(templateId));
			}

			return child;
		}

		/// <summary>
		/// Determine if a particular template is derived from a specific template.
		/// </summary>
		/// <param name="template">The template being examined.</param>
		/// <param name="templateToCompare">The proposed ancestor.</param>
		/// <returns>True if template matches.</returns>
		private static bool TemplateIsDerivedFrom(TemplateItem template, TemplateItem templateToCompare)
		{
			foreach (var baseTemplate in template.BaseTemplates)
			{
				if (baseTemplate.ID == templateToCompare.ID)
				{
					return true;
				}

				if (TemplateIsDerivedFrom(baseTemplate, templateToCompare))
				{
					return true;
				}
			}

			return false;
		}
	}
}