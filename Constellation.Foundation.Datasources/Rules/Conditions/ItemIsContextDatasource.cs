using Constellation.Foundation.Data;
using Sitecore.Data;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Constellation.Foundation.Datasources.Rules.Conditions
{
	/// <summary>
	/// Rule Condition that determines if the supplied Item descends from the Context Datasource Template.
	/// </summary>
	/// <typeparam name="T">The RuleContext</typeparam>
	public class ItemIsContextDatasource<T> : WhenCondition<T> where T : RuleContext
	{
		/// <summary>
		/// The ID of the Context Datasource Template
		/// </summary>
		// ReSharper disable once InconsistentNaming
		protected readonly ID ContextDatasourceID = new ID("{A3FC46C6-CD56-4F1D-9E5B-1187592CAE0A}");

		/// <summary>
		/// Called when Sitecore executes the Rule Condition.
		/// </summary>
		/// <param name="ruleContext">The rule context.</param>
		/// <returns>Returns true if the supplied Item descends from the Context Datasource template.</returns>
		protected override bool Execute(T ruleContext)
		{
			var item = ruleContext.Item;

			return item.IsDerivedFrom(ContextDatasourceID);
		}
	}
}
