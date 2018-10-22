using System.Globalization;
using Constellation.Foundation.Data;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Rules;
using Sitecore.Rules.Actions;
using Sitecore.SecurityModel;

namespace Constellation.Feature.ItemSorting.Rules.Actions
{
	/// <summary>
	/// Considers the name of the specified item and if it's not already,
	/// moves it into an alphabetical folder based upon
	/// the first letter of its Name property. This movement is relative to the nearest
	/// root item of the alphabetical hierarchy.
	/// </summary>
	/// <typeparam name="T">The Rule Context</typeparam>
	public class MoveToAlphabeticalFolder<T> : RuleAction<T>
		where T : RuleContext
	{
		#region Properties

		/// <summary>
		/// Gets or sets the Folder template.
		/// </summary>
		public string FolderTemplate { get; set; }
		#endregion

		/// <summary>
		/// Sitecore calls this method when the Rule Action needs to be executed.
		/// Determines the first letter of the provided Item's name. Finds or creates an Item
		/// named after that letter one level above the provided Item. Moves the provided Item
		/// into that folder.
		/// </summary>
		/// <param name="ruleContext">The Rule Context.</param>
		public override void Apply(T ruleContext)
		{
			if (ruleContext.Item.TemplateID == new ID(this.FolderTemplate))
			{
				return;
			}

			var item = ruleContext.Item;
			var name = item.Name.ToUpper(CultureInfo.InvariantCulture);
			var firstLetter = name[0].ToString(CultureInfo.InvariantCulture);

			using (new SecurityDisabler())
			{
				var rootFolder = this.GetOrganizingRoot(item);
				var alphaFolder = rootFolder.FindOrCreateChildItem(firstLetter, new ID(this.FolderTemplate));
				item.MoveTo(alphaFolder);
			}
		}


		/// <summary>
		/// Gets the root item for a given item.
		/// </summary>
		/// <param name="context">
		/// The context item.
		/// </param>
		/// <returns>
		/// The root item<see cref="Item"/>.
		/// </returns>
		private Item GetOrganizingRoot(Item context)
		{
			if (context.Parent.TemplateID != new ID(this.FolderTemplate))
			{
				return context.Parent;
			}

			return this.GetOrganizingRoot(context.Parent);
		}
	}
}
