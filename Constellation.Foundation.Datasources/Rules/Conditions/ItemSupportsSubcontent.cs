using Constellation.Foundation.Data;
using Sitecore.Data;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Constellation.Foundation.Datasources.Rules.Conditions
{
	public class ItemSupportsSubcontent<T> : WhenCondition<T> where T : RuleContext
	{
		// ReSharper disable once InconsistentNaming
		protected readonly ID SupportsSubcontentID = new ID("{32D86944-A559-4313-8D5D-5867EA246DF5}");

		protected override bool Execute(T ruleContext)
		{
			var item = ruleContext.Item;

			return item.IsDerivedFrom(SupportsSubcontentID);
		}


	}
}
