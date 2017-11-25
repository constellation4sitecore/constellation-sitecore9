using Constellation.Foundation.Data;
using Sitecore.Data;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Constellation.Foundation.Datasources.Rules.Conditions
{
	public class ItemIsSubcontentDatasource<T> : WhenCondition<T> where T : RuleContext
	{
		// ReSharper disable once InconsistentNaming
		protected readonly ID SubcontentDatasourceID = new ID("{43160509-4185-4DFF-9CA9-7862AE31E4F8}");

		protected override bool Execute(T ruleContext)
		{
			var item = ruleContext.Item;

			return item.IsDerivedFrom(SubcontentDatasourceID);
		}
	}
}
