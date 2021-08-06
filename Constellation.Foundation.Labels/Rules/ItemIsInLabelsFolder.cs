using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Constellation.Foundation.Labels.Rules
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ItemIsInLabelsFolder<T> : WhenCondition<T> where T : RuleContext
	{
		/// <summary>
		/// Called when Sitecore executes the Rule Condition.
		/// </summary>
		/// <param name="ruleContext">The rule context.</param>
		/// <returns>Returns true if the supplied Item descends from the Context Datasource template.</returns>
		protected override bool Execute(T ruleContext)
		{
			var item = ruleContext.Item;

			var labelFolderName = Sitecore.Configuration.Settings.GetSetting(SettingNames.LabelFolderName);

			if (item.Name.Equals(labelFolderName))
			{
				return false;
			}

			if (string.IsNullOrEmpty(labelFolderName))
			{
				Log.Warn($"Foundation.Labels - ItemIsInLabelsFolder requires an XML setting for {SettingNames.LabelFolderName} in order to execute. Check your cnofig file.", this);
				return false;
			}

			return item.Paths.FullPath.Contains(labelFolderName);
		}
	}
}
