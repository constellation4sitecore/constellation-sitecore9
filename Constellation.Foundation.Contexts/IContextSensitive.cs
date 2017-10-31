namespace Constellation.Foundation.Contexts
{
	/// <summary>
	/// Contract specifying the properties required to test whether a given object
	/// should execute during a Sitecore event or pipeline, such as a rule, event handler, or pipeline step.
	/// </summary>
	public interface IContextSensitive
	{
		/// <summary>
		/// Gets the Context Validator associated with the instance.
		/// </summary>
		ContextValidator ContextValidator { get; }

		/// <summary>
		/// Gets the Sitecore database name for the current request.
		/// </summary>
		string ContextDatabaseName { get; }

		/// <summary>
		/// Gets the Host name of the current request.
		/// </summary>
		string ContextHostName { get; }

		/// <summary>
		/// Gets the file path for the current request.
		/// </summary>
		string ContextLocalPath { get; }

		/// <summary>
		/// Gets the Sitecore site name for the current request.
		/// </summary>
		string ContextSiteName { get; }

		/// <summary>
		/// <para>
		/// Gets or sets a comma-delimited list of Database names that should match the current Request before
		/// the Processor executes.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		string DatabasesToProcess { get; set; }

		/// <summary>
		/// <para>
		/// Gets or sets a comma-delimited list of Database names that should cause the Processor to ignore
		/// the request.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		string DatabasesToIgnore { get; set; }

		/// <summary>
		/// <para>
		/// Gets or sets a comma-delimited list of host names that should match the current Request before
		/// the Processor executes.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		string HostnamesToProcess { get; set; }

		/// <summary>
		/// <para>
		/// Gets or sets a comma-delimited list of host names that should cause the Processor to ignore
		/// the request.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		string HostnamesToIgnore { get; set; }

		/// <summary>
		/// <para>
		/// Gets or sets a comma-delimited list of file paths that should match the current Request before
		/// the Processor executes.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		string PathsToProcess { get; set; }

		/// <summary>
		/// <para>
		/// Gets or sets a comma-delimited list of file paths that should cause the Processor to ignore 
		/// the request.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		string PathsToIgnore { get; set; }

		/// <summary>
		/// <para>
		/// Gets or sets a comma-delimited list of Site names that should match the current Request before
		/// the Processor executes.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		string SitesToProcess { get; set; }

		/// <summary>
		/// <para>
		/// Gets or sets a comma-delimited list of Site names that should cause the Processor to ignore
		/// the request.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		string SitesToIgnore { get; set; }
	}
}