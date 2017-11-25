using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Actions;
using Sitecore.SecurityModel;

namespace Constellation.Foundation.Datasources.Rules.Actions
{
	/// <summary>
	/// Creates the obligatory Subcontent folder required for SupportsSubcontent Items.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class CreateSubcontentFolder<T> : RuleAction<T>
		where T : RuleContext
	{
		// ReSharper disable once InconsistentNaming
		protected readonly ID SubcontentFolderID = new ID("{3C8F34CC-2C10-4C2D-B7CC-74CA2F4FB341}");

		public override void Apply(T ruleContext)
		{
			using (new SecurityDisabler())
			{
				// See if there's already a folder
				var children = ruleContext.Item.GetChildren();

				// Yes, it might seem a little slow, but in reality processing 1000 items this way takes a few miliseconds.
				foreach (Item child in children)
				{
					if (child.TemplateID == SubcontentFolderID)
					{
						Log.Info("Item already has a Subcontent folder.", this);
						return; // Nothing to do.
					}
				}

				var item = ruleContext.Item;

				var master = Sitecore.Configuration.Factory.GetDatabase("master");
				var template = master.GetTemplate(SubcontentFolderID);

				var folder = item.Add("_subcontent", template);

				Log.Info($"{folder.Paths.FullPath} created.", this);
			}
		}
	}
}
