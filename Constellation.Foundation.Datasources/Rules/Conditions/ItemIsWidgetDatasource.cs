using Constellation.Foundation.Data;
using Sitecore.Data;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Constellation.Foundation.Datasources.Rules.Conditions
{
	public class ItemIsWidgetDatasource<T> : WhenCondition<T> where T : RuleContext
	{
		// ReSharper disable once InconsistentNaming
		protected readonly ID WidgetDatasourceID = new ID("{78208674-1BF2-4A6B-82DF-2C9210D6E305}");

		protected override bool Execute(T ruleContext)
		{
			var item = ruleContext.Item;

			return item.IsDerivedFrom(WidgetDatasourceID);
		}
	}
}
