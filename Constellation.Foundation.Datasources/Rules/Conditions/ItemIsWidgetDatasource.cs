using Constellation.Foundation.Data;
using Sitecore.Data;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Constellation.Foundation.Datasources.Rules.Conditions
{
	/// <summary>
	/// Rule Condition that determines if the supplied Item descends from the Widget Datasource template.
	/// </summary>
	/// <typeparam name="T">The Rule Context.</typeparam>
	public class ItemIsWidgetDatasource<T> : WhenCondition<T> where T : RuleContext
	{
		/// <summary>
		/// The ID of the Widget Datasource Template.
		/// </summary>
		// ReSharper disable once InconsistentNaming
		protected readonly ID WidgetDatasourceID = new ID("{78208674-1BF2-4A6B-82DF-2C9210D6E305}");

		/// <summary>
		/// Called by Sitecore when executing the Rule Condition.
		/// </summary>
		/// <param name="ruleContext">The rule context.</param>
		/// <returns>True if the supplied Item descends from the Widget Datasource template.</returns>
		protected override bool Execute(T ruleContext)
		{
			var item = ruleContext.Item;

			return item.IsDerivedFrom(WidgetDatasourceID);
		}
	}
}
