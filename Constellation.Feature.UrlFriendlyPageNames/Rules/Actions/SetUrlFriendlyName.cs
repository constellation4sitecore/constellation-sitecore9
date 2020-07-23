using System.Diagnostics.CodeAnalysis;
using Sitecore;
using Sitecore.Data.Items;

namespace Constellation.Feature.UrlFriendlyPageNames.Rules.Actions
{
	/// <summary>
	/// A Sitecore Rule Action that adjusts the name of an Item to support the desired URL-friendly parameters.
	/// Parameters are defined by the user in the calling Rule.
	/// </summary>
	/// <typeparam name="T">Instance of Sitecore.Rules.RuleContext.</typeparam>
	public class SetUrlFriendlyName<T> : Constellation.Foundation.Contexts.Rules.ContextSensitiveRuleAction<T>
		 where T : global::Sitecore.Rules.RuleContext
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="SetUrlFriendlyName{T}"/> class.
		/// </summary>
		public SetUrlFriendlyName()
		{
			this.DatabasesToProcess = "master";
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets a value indicating whether to replace diacritics from name or not.
		/// </summary>
		public bool ReplaceDiacritics { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether saving the new Name value should also clear 
		/// the DisplayName of the Item.
		/// </summary>
		public bool ClearDisplayName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating the list of illegal characters that should be stripped from the Item's name.
		/// Format should be .NET compatible Regex.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "RegEx is a legitamate name.")]
		public string IllegalCharacterRegEx { get; set; }

		/// <summary>
		/// Gets or sets a value indicating the desired behavior when the name builder encounters a space " " character.
		/// Options are removal or replacement with a dash "-".
		/// </summary>
		public ItemNameManager.SpaceHandling RemoveSpaces { get; set; }

		/// <summary>
		/// Gets or sets a value indicating the desired behavior when the name builder rewrites a name containing uppercase
		/// characters.
		/// Options are force lowercase or preserve existing case.
		/// </summary>
		public ItemNameManager.CaseHandling ChangeCase { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// Called when Sitecore executes the Action.
		/// </summary>
		/// <param name="ruleContext">An instance of Sitecore.Rules.RuleContext.</param>
		protected override void Execute(T ruleContext)
		{
			if (ItemNameManager.IsTargetForRenaming(ruleContext.Item))
			{
				string name;
				if (ItemNameManager.GetLocallyUniqueItemName(ruleContext.Item, this.IllegalCharacterRegEx, this.RemoveSpaces, this.ChangeCase, this.ReplaceDiacritics, out name))
				{
					using (new EditContext(ruleContext.Item, false, false))
					{
						ruleContext.Item.Name = name;

						if (this.ClearDisplayName)
						{
							ruleContext.Item.Fields[FieldIDs.DisplayName].Value = string.Empty;
						}
					}
				}
			}
		}
		#endregion
	}
}
