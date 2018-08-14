namespace Constellation.Foundation.Contexts.Pipelines
{
	using System.Diagnostics.CodeAnalysis;
	using System.Web;
	using Contexts;
	using Sitecore.Diagnostics;
	using Sitecore.Pipelines.HttpRequest;

	/// <summary>
	/// Basic implementation of IContextSensitive for use with HttpPipeline processors.
	/// </summary>
	/// <remarks>
	/// This should be the starting place of any HttpRequestPipeline processor, because context is 
	/// extremely important to this pipeline. Use the configuration files in Sitecore to ensure that
	/// the restricting properties provided by IContextSensitive are populated to prevent your pipeline
	/// processor from firing when you don't want it to (for example, for Sitecore Shell requests)
	/// </remarks>
	public abstract class ContextSensitiveHttpRequestProcessor : HttpRequestProcessor, IContextSensitive
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="HttpRequestProcessor"/> class.
		/// </summary>
		// ReSharper disable PublicConstructorInAbstractClass
		public ContextSensitiveHttpRequestProcessor()
		// ReSharper restore PublicConstructorInAbstractClass
		{
			this.ContextValidator = new ContextValidator(this);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the Context Validator associated with the instance.
		/// </summary>
		public ContextValidator ContextValidator { get; }

		/// <summary>
		/// Gets the Sitecore database name for the current request.
		/// </summary>
		public string ContextDatabaseName
		{
			get { return Sitecore.Context.Database != null ? Sitecore.Context.Database.Name : string.Empty; }
		}

		/// <summary>
		/// Gets the Host name of the current request.
		/// </summary>
		public string ContextHostName
		{
			get { return HttpContext.Current != null ? HttpContext.Current.Request.Url.Host : string.Empty; }
		}

		/// <summary>
		/// Gets the file path for the current request.
		/// </summary>
		public string ContextLocalPath
		{
			get { return HttpContext.Current != null ? HttpContext.Current.Request.Url.LocalPath : string.Empty; }
		}

		/// <summary>
		/// Gets the Sitecore site name for the current request.
		/// </summary>
		public string ContextSiteName
		{
			get { return Sitecore.Context.Site != null ? Sitecore.Context.Site.Name : string.Empty; }
		}

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
		/// <para>
		/// Recommended Values:
		/// "core"
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
		/// <para>
		/// Recommended Values:
		/// "/~", "/maintenance", "/sitecore", "/sitecore modules", "/_DEV/TdsService.asmx"
		/// </para>
		/// </summary>
		[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
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
		/// <para>
		/// Recommended Values:
		/// "admin, login, modules_shell, modules_website, publisher, scheduler, service, shell, system, testing"
		/// </para>
		/// </summary>
		public string SitesToIgnore { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// Sitecore's contract for Pipeline Processors.
		/// </summary>
		/// <param name="args">The details of the current HttpRequest.</param>
		public override void Process(HttpRequestArgs args)
		{
			if (this.ContextValidator.ContextIsValidForExecution())
			{
				this.Execute(args);
			}
			else
			{
				Log.Debug("Processing deferred because it is not executing the correct context", this);
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

				this.Defer(args);
			}
		}

		/// <summary>
		/// Method that is executed if the conditions defined in the properties
		/// determine that the Processor should run for this request.
		/// Implement the behavior of the processor in this method.
		/// </summary>
		/// <param name="args">The details of the current HttpRequest.</param>
		protected abstract void Execute(HttpRequestArgs args);

		/// <summary>
		/// Method that is executed if the conditions defined in the properties
		/// determine that the Processor should NOT run fro this request.
		/// Use this method to allow stock Sitecore processes to run when the
		/// custom processor is not a good idea.
		/// </summary>
		/// <param name="args">The details of the current HttpRequest.</param>
		protected abstract void Defer(HttpRequestArgs args);
		#endregion
	}
}