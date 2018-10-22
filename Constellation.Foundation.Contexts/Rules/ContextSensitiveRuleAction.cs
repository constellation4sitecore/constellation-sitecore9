namespace Constellation.Foundation.Contexts.Rules
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Constellation.Foundation.Contexts;
	using Sitecore.Data.Items;
	using Sitecore.Diagnostics;
	using Sitecore.Sites;
	using Sitecore.Web;

	/// <summary>
	/// A Base for all custom Rule Actions that incorporates context sensitivity as well as preventing recursive rule running.
	/// </summary>
	/// <remarks>
	/// Context analysis occurs when Apply() is called, which is then handed off to the Execute() method, which must
	/// be implemented by the inheriting class. Note that for Rules, valid context values can be assigned in the
	/// rule definition by including them in the rule's parameters. via the Rule Action Item's Text field. Example follows:
	/// 
	/// Execute Rule only when Database is [DatabasesToProcess,,,, database name] and Item is a member of [SitesToProcess,,,, site name]
	/// 
	/// For rule actions, this may seem redundant as some of these decisions can be made via rule conditions, but it occasionally comes
	/// in handy. Because rules can only execute in a few scenarios, some of the IContextSensitive conditions may not apply.
	/// 
	/// Rule recursion prevention also occurs when Apply() is called. Per cycle, the rule is only executed if the rule is not 
	/// currently being executed on the same ID.
	/// </remarks>
	/// <typeparam name="T">The ruleContext.</typeparam>
	public abstract class ContextSensitiveRuleAction<T> : Sitecore.Rules.Actions.RuleAction<T>, IContextSensitive
		where T : Sitecore.Rules.RuleContext
	{
		#region Locals
		/// <summary>
		/// Lists IDs that are still being acted upon. Prevents recursion.
		/// </summary>
		// ReSharper disable StaticFieldInGenericType
		private static readonly List<string> InProgress = new List<string>();
		// ReSharper restore StaticFieldInGenericType
		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ContextSensitiveRuleAction{T}"/> class.
		/// </summary>
		// ReSharper disable PublicConstructorInAbstractClass
		public ContextSensitiveRuleAction()
		// ReSharper restore PublicConstructorInAbstractClass
		{
			this.ContextValidator = new ContextValidator(this);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the Context Validator associated with the instance.
		/// </summary>
		public ContextValidator ContextValidator { get; private set; }

		/// <summary>
		/// Gets the Sitecore database name for the current request.
		/// </summary>
		public string ContextDatabaseName { get; private set; }

		/// <summary>
		/// Gets the Host name of the current request.
		/// </summary>
		public string ContextHostName { get; private set; }

		/// <summary>
		/// Gets the file path for the current request.
		/// </summary>
		public string ContextLocalPath { get; private set; }

		/// <summary>
		/// Gets the Sitecore site name for the current request.
		/// </summary>
		public string ContextSiteName { get; private set; }

		/// <summary>
		/// <para>
		/// Gets or sets a comma-delimited list of Database names that should match the current Request before
		/// the Processor executes.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		public string DatabasesToProcess { get; set; }

		/// <summary>
		/// <para>
		/// Gets or sets a comma-delimited list of Database names that should cause the Processor to ignore
		/// the request.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		public string DatabasesToIgnore { get; set; }

		/// <summary>
		/// <para>
		/// Gets or sets a comma-delimited list of host names that should match the current Request before
		/// the Processor executes.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		public string HostnamesToProcess { get; set; }

		/// <summary>
		/// <para>
		/// Gets or sets a comma-delimited list of host names that should cause the Processor to ignore
		/// the request.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		public string HostnamesToIgnore { get; set; }

		/// <summary>
		/// <para>
		/// Gets or sets a comma-delimited list of file paths that should match the current Request before
		/// the Processor executes.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		public string PathsToProcess { get; set; }

		/// <summary>
		/// <para>
		/// Gets or sets a comma-delimited list of file paths that should cause the Processor to ignore 
		/// the request.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		public string PathsToIgnore { get; set; }

		/// <summary>
		/// <para>
		/// Gets or sets a comma-delimited list of Site names that should match the current Request before
		/// the Processor executes.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		public string SitesToProcess { get; set; }

		/// <summary>
		/// <para>
		/// Gets or sets a comma-delimited list of Site names that should cause the Processor to ignore
		/// the request.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		public string SitesToIgnore { get; set; }
		#endregion

		#region Methods

		/// <summary>
		/// The method called by the Rule Engine.
		/// </summary>
		/// <param name="ruleContext">The context.</param>
		public override void Apply(T ruleContext)
		{
			if (!this.ContextValidator.ContextIsValidForExecution())
			{
				Log.Info("RuleAction not applied because it was executed in the wrong context", this);

				if (Log.IsDebugEnabled)
				{
					Log.Debug($"		ContextDatabase: {this.ContextDatabaseName}");
					Log.Debug($"		ContextSiteName: {this.ContextSiteName}");
					Log.Debug($"		ContextHostName: {this.ContextHostName}");
					Log.Debug($"		ContextLocalPath: {this.ContextLocalPath}");
					Log.Debug($"		AllowedDatabase: {this.DatabasesToProcess}");
					Log.Debug($"		AllowedSiteName: {this.SitesToProcess}");
					Log.Debug($"		AllowedHostName: {this.HostnamesToProcess}");
					Log.Debug($"		AllowedLocalPath: {this.PathsToProcess}");
					Log.Debug($"		IgnoredDatabase: {this.DatabasesToIgnore}");
					Log.Debug($"		IgnoredSiteName: {this.SitesToIgnore}");
					Log.Debug($"		IgnoredHostName: {this.HostnamesToIgnore}");
					Log.Debug($"		IgnoredLocalPath: {this.PathsToIgnore}");
				}
				return;
			}

			if (InProgress.Contains(ruleContext.Item.ID.ToString()))
			{
				Log.Info($"RuleAction not applied because a parent instance of the same rule is executing on {ruleContext.Item.Name}.", this);
				return;
			}

			Log.Debug($"RuleAction started for {ruleContext.Item.Name}", this);
			InProgress.Add(ruleContext.Item.ID.ToString());

			try
			{
				this.Execute(ruleContext);
			}
			catch (Exception ex)
			{
				Log.Error($"RuleAction failed for {ruleContext.Item.Name}", ex, this);
			}
			finally
			{
				InProgress.Remove(ruleContext.Item.ID.ToString());
			}

			Log.Debug($"RuleAction ended for {ruleContext.Item.Name}", this);
		}

		/// <summary>
		/// Implement the Action details.
		/// </summary>
		/// <param name="ruleContext">The context.</param>
		protected abstract void Execute(T ruleContext);

		/// <summary>
		/// Sets the context properties based on the provided Item.
		/// </summary>
		/// <param name="item">The Item to derive context from.</param>
		protected void SetContextProperties(Item item)
		{
			this.ContextDatabaseName = item.Database.Name;

			string itemPath = item.Paths.FullPath;
			SiteInfo site = SiteContextFactory.Sites
				.Where(s => s.RootPath != "" & itemPath.StartsWith(s.RootPath, StringComparison.OrdinalIgnoreCase))
				.OrderByDescending(s => s.RootPath.Length)
				.FirstOrDefault();

			if (site != null)
			{
				ContextSiteName = site.Name;
			}
		}
		#endregion
	}
}