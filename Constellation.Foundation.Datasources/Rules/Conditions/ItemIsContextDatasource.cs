using Constellation.Foundation.Data;
using Sitecore.Data;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Constellation.Foundation.Datasources.Rules.Conditions
{
	public class ItemIsContextDatasource<T> : WhenCondition<T> where T : RuleContext
	{
		// ReSharper disable once InconsistentNaming
		protected readonly ID ContextDatasourceID = new ID("{A3FC46C6-CD56-4F1D-9E5B-1187592CAE0A}");

		protected override bool Execute(T ruleContext)
		{
			var item = ruleContext.Item;

			return item.IsDerivedFrom(ContextDatasourceID);
		}
	}
}
