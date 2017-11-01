using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Constellation.Foundation.Items.Rules.Conditions
{
	/// <summary>
	/// Rule that walks the supplied Item's template inheritance hierarchy looking
	/// for a match to the TemplateID provided by the rule context.
	/// </summary>
	/// <typeparam name="T">Instance of Sitecore.Rules.Conditions.RuleContext.</typeparam>
	public class ItemDerivesFromTemplate<T> : WhenCondition<T>
		where T : RuleContext
	{
		/// <summary>
		/// The template Id.
		/// </summary>
		private ID _templateId;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the ItemDerivesFromTemplate class.
		/// </summary>
		public ItemDerivesFromTemplate()
		{
			_templateId = ID.Null;
		}

		/// <summary>
		/// Initializes a new instance of the ItemDerivesFromTemplate class.
		/// </summary>
		/// <param name="templateId">The template id.</param>
		public ItemDerivesFromTemplate(ID templateId)
		{
			Assert.ArgumentNotNull(templateId, "templateId");
			_templateId = templateId;
		}
		#endregion

		/// <summary>
		/// Gets or sets the template id.
		/// </summary>
		public ID TemplateId
		{
			get => _templateId;

			set
			{
				Assert.ArgumentNotNull(value, "value");
				_templateId = value;
			}
		}

		/// <summary>
		/// Executes the specified rule context.
		/// </summary>
		/// <param name="ruleContext">The rule context.</param>
		/// <returns>
		/// <c>True</c>, if the condition succeeds, otherwise <c>false</c>.
		/// </returns>
		protected override bool Execute(T ruleContext)
		{
			Assert.ArgumentNotNull(ruleContext, "ruleContext");
			var item = ruleContext.Item.AsStronglyTyped();
			if (item == null)
			{
				return false;
			}

			var template = ItemFactory.GetTemplateInterfaceType(TemplateId);
			return template != null && template.IsInstanceOfType(item);
		}
	}
}
