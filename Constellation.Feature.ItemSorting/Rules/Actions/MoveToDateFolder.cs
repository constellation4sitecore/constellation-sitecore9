using System.Globalization;
using Constellation.Foundation.Data;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Rules;
using Sitecore.Rules.Actions;
using Sitecore.SecurityModel;

namespace Constellation.Feature.ItemSorting.Rules.Actions
{
	/// <summary>
	/// Considers the specified Date field of the supplied Item and if it's not already,
	/// moves it into a folder structure based upon the value of the specified Date field.
	/// This movement is relative to the nearest root item of the date-based hierarchy.
	/// </summary>
	/// <typeparam name="T">The Rule Context</typeparam>
	public class MoveToDateFolder<T> : RuleAction<T>
		where T : RuleContext
	{
		/// <summary>
		/// The date sort options.
		/// </summary>
		public enum DateSortOptions
		{
			/// <summary>
			/// The only year.
			/// </summary>
			OnlyYear,

			/// <summary>
			/// The year and month.
			/// </summary>
			YearAndMonth,

			/// <summary>
			/// The year and month and day.
			/// </summary>
			YearAndMonthAndDay
		}


		#region Properties
		/// <summary>
		/// Gets or sets the Folder template.
		/// </summary>
		public string FolderTemplate { get; set; }

		/// <summary>
		/// Gets or sets the name of the field on the target item
		/// that contains the date value to use for sorting.
		/// </summary>
		public string NameOfDateFieldToSortBy { get; set; }

		/// <summary>
		/// Gets or sets the folder depth.
		/// </summary>
		public DateSortOptions FolderDepth { get; set; }

		#endregion


		/// <summary>
		/// Sitecore calls this method when the Rule Action needs to be executed.
		/// Determines the date value of the specified Field on the specified Item.
		/// Finds or creates a date-based folder structure of the appropriate depth (based on the rule's settings).
		/// Moves the specified Item into the date-based folder structure.
		/// </summary>
		/// <param name="ruleContext">The Rule Context.</param>
		public override void Apply(T ruleContext)
		{
			var item = ruleContext.Item;
			if (item.TemplateID == new ID(this.FolderTemplate))
			{
				return;
			}

			DateField field = item.Fields[NameOfDateFieldToSortBy] ?? item.Fields["__Created"];
			if (field.DateTime == System.DateTime.MinValue)
			{
				return;
			}

			var theYear = field.DateTime.ToString("yyyy", CultureInfo.InvariantCulture);
			var theMonth = field.DateTime.ToString("MM", CultureInfo.InvariantCulture);
			var theDay = field.DateTime.ToString("dd", CultureInfo.InvariantCulture);

			var folderLevel = this.GetOrganizingRoot(item);
			var oldFilePath = item.Paths.FullPath;
			var datePath = "/" + theYear + "/";
			if (!oldFilePath.Contains(datePath))
			{
				this.MoveItem(theYear, item, folderLevel);
				oldFilePath = item.Paths.FullPath;
			}

			if (this.FolderDepth != DateSortOptions.OnlyYear)
			{
				datePath = datePath + theMonth + "/";
				if (!oldFilePath.Contains(datePath))
				{
					folderLevel = this.AdvanceFolderLevel(folderLevel, item);
					this.MoveItem(theMonth, item, folderLevel);
					oldFilePath = item.Paths.FullPath;
				}

				if (this.FolderDepth == DateSortOptions.YearAndMonthAndDay)
				{
					datePath = datePath + theDay + "/";
					if (!oldFilePath.Contains(datePath))
					{
						folderLevel = this.AdvanceFolderLevel(folderLevel, item);
						this.MoveItem(theDay, item, folderLevel);
					}
				}
			}
		}

		#region Helpers
		/// <summary>
		/// Finds the child of a folder that is an ancestor of an item.
		/// </summary>
		/// <param name="folder">
		/// The folder.
		/// </param>
		/// <param name="item">
		/// The context item.
		/// </param>
		/// <returns>
		/// The correct folder<see cref="Item"/>.
		/// </returns>
		protected Item AdvanceFolderLevel(Item folder, Item item)
		{
			foreach (Item child in folder.GetChildren())
			{
				if (child.Axes.IsAncestorOf(item))
				{
					return child;
				}
			}

			return folder;
		}

		/// <summary>
		/// Gets the root of the hierarchy
		/// </summary>
		/// <param name="item">
		/// The context item.
		/// </param>
		/// <returns>
		/// The site folder<see cref="Item"/> for the item.
		/// </returns>
		protected Item GetOrganizingRoot(Item item)
		{
			item = item.Parent;
			while (item.TemplateID.ToString() == this.FolderTemplate)
			{
				item = item.Parent;
			}

			return item;
		}

		/// <summary>
		/// Moves the item to a new folder.
		/// </summary>
		/// <param name="name">
		/// The name of the folder.
		/// </param>
		/// <param name="currentItem">
		/// The current item.
		/// </param>
		/// <param name="folderLevel">
		/// The folder Level.
		/// </param>
		protected void MoveItem(string name, Item currentItem, Item folderLevel)
		{
			using (new SecurityDisabler())
			{
				var newFolder = folderLevel.FindOrCreateChildItem(name, new ID(this.FolderTemplate));
				currentItem.MoveTo(newFolder);
			}
		}
		#endregion
	}
}
