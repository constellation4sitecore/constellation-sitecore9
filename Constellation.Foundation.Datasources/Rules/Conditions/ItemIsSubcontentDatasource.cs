using Constellation.Foundation.Data;
using Sitecore.Data;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Constellation.Foundation.Datasources.Rules.Conditions
{
	/// <summary>
	/// A rule condition that checks to see if the supplied Item inherits from the Subcontent Datasource Template.
	/// </summary>
	/// <typeparam name="T">The Rule Context arguments.</typeparam>
	public class ItemIsSubcontentDatasource<T> : WhenCondition<T> where T : RuleContext
	{
		/// <summary>
		/// The ID of the Subcontent Datasource Template.
		/// </summary>
		// ReSharper disable once InconsistentNaming
		protected readonly ID SubcontentDatasourceID = new ID("{43160509-4185-4DFF-9CA9-7862AE31E4F8}");

		/// <summary>
		/// Called when Sitecore executes the Rule Condition.
		/// Checks to see if the supplied Item inherits from the Subcontent Datasource Template.
		/// </summary>
		/// <param name="ruleContext"></param>
		/// <returns></returns>
		protected override bool Execute(T ruleContext)
		{
			var item = ruleContext.Item;

			return item.IsDerivedFrom(SubcontentDatasourceID);
		}
	}
}
