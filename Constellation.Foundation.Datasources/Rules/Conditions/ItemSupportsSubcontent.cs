using Constellation.Foundation.Data;
using Sitecore.Data;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Constellation.Foundation.Datasources.Rules.Conditions
{
	/// <summary>
	/// Rule Condition that determines if the supplied Item descends from the Supports Subcontent Template.
	/// </summary>
	/// <typeparam name="T">The rule context.</typeparam>
	public class ItemSupportsSubcontent<T> : WhenCondition<T> where T : RuleContext
	{
		/// <summary>
		/// The ID of the Supports Subcontent Template.
		/// </summary>
		// ReSharper disable once InconsistentNaming
		protected readonly ID SupportsSubcontentID = new ID("{32D86944-A559-4313-8D5D-5867EA246DF5}");

		/// <summary>
		/// Called by Sitecore when the Rule Condition is executed.
		/// </summary>
		/// <param name="ruleContext">The rule context.</param>
		/// <returns>True if the supplied Item descends from the Supports Subcontent Template.</returns>
		protected override bool Execute(T ruleContext)
		{
			var item = ruleContext.Item;

			return item.IsDerivedFrom(SupportsSubcontentID);
		}


	}
}
