using Constellation.Feature.StaticNavigation.Models;
using Constellation.Foundation.ModelMapping;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Constellation.Feature.StaticNavigation
{
	public class SimpleNavigationController : Constellation.Foundation.Mvc.ConventionController
	{
		protected override object GetModel(Item datasource, Item contextItem)
		{

			var model = new SimpleNavigation();

			model.DisplayName = datasource.DisplayName;

			var items = datasource.GetChildren();

			foreach (Item item in items)
			{
				var link = item.MapToNew<NavigationLink>();

				if (link.UseThisDisplayName)
				{
					link.DisplayName = item.DisplayName;
					model.Links.Add(link);
					continue;
				}

				LinkField field = item.Fields[FieldIDs.Link];

				if (!string.IsNullOrEmpty(field.Text))
				{
					link.DisplayName = field.Text;
					model.Links.Add(link);
					continue;
				}

				if (field.IsInternal && field.TargetItem != null)
				{
					link.DisplayName = field.TargetItem[FieldIDs.NavigationTitle];

					if (string.IsNullOrEmpty(link.DisplayName))
					{
						link.DisplayName = field.TargetItem.DisplayName;
						model.Links.Add(link);
						continue;
					}
				}

				link.DisplayName = item.DisplayName;
				model.Links.Add(link);
			}

			return model;
		}
	}
}
