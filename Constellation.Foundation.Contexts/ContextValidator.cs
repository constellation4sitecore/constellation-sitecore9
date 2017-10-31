namespace Constellation.Foundation.Contexts
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Evaluates IContextSensitive objects to determine if they should execute.
	/// </summary>
	public class ContextValidator
	{
		private readonly IContextSensitive target;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ContextValidator"/> class.
		/// </summary>
		/// <param name="target">The object that requires evaluation.</param>
		public ContextValidator(IContextSensitive target)
		{
			this.target = target;
		}
		#endregion

		#region Properties
		/// <summary>
		/// <para>
		/// Gets a list of Database names that should match the current Request before
		/// the Processor executes.
		/// </para>
		/// </summary>
		protected ICollection<string> DatabasesToProcess
		{
			get
			{
				return ConvertToList(target.DatabasesToProcess);
			}
		}

		/// <summary>
		/// <para>
		/// Gets a list of Database names that should cause the Processor to ignore
		/// the request.
		/// </para>
		/// </summary>
		protected ICollection<string> DatabasesToIgnore
		{
			get
			{
				return ConvertToList(target.DatabasesToIgnore);
			}
		}

		/// <summary>
		/// <para>
		/// Gets a list of host names that should match the current Request before
		/// the Processor executes.
		/// </para>
		/// </summary>
		protected ICollection<string> HostnamesToProcess
		{
			get
			{
				return ConvertToList(target.HostnamesToProcess);
			}
		}

		/// <summary>
		/// <para>
		/// Gets a list of host names that should cause the Processor to ignore
		/// the request.
		/// </para>
		/// </summary>
		protected ICollection<string> HostnamesToIgnore
		{
			get
			{
				return ConvertToList(target.HostnamesToIgnore);
			}
		}

		/// <summary>
		/// <para>
		/// Gets a list of file paths that should match the current Request before
		/// the Processor executes.
		/// </para>
		/// </summary>
		protected ICollection<string> PathsToProcess
		{
			get
			{
				return ConvertToList(target.PathsToProcess);
			}
		}

		/// <summary>
		/// <para>
		/// Gets a list of file paths that should cause the Processor to ignore 
		/// the request.
		/// </para>
		/// </summary>
		protected ICollection<string> PathsToIgnore
		{
			get
			{
				return ConvertToList(target.PathsToIgnore);
			}
		}

		/// <summary>
		/// <para>
		/// Gets a list of Site names that should match the current Request before
		/// the Processor executes.
		/// </para>
		/// </summary>
		protected ICollection<string> SitesToProcess
		{
			get
			{
				return ConvertToList(target.SitesToProcess);
			}
		}

		/// <summary>
		/// <para>
		/// Gets a list of Site names that should cause the Processor to ignore
		/// the request.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		protected ICollection<string> SitesToIgnore
		{
			get
			{
				return ConvertToList(target.SitesToIgnore);
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Determines if the processor should execute based upon its context and
		/// its whitelist/blacklist for context properties.
		/// </summary>
		/// <returns>True if the processor should execute based upon the constraints provided.</returns>
		public bool ContextIsValidForExecution()
		{
			if (!PassesRuleTest(HostnamesToProcess, HostnamesToIgnore, target.ContextHostName))
			{
				return false;
			}

			if (!PassesRuleTest(PathsToProcess, PathsToIgnore, target.ContextLocalPath))
			{
				return false;
			}

			if (!PassesRuleTest(SitesToProcess, SitesToIgnore, target.ContextSiteName))
			{
				return false;
			}

			if (!PassesRuleTest(DatabasesToProcess, DatabasesToIgnore, target.ContextDatabaseName))
			{
				return false;
			}

			return true;
		}
		#endregion

		#region Rule Processing
		/// <summary>
		/// Performs the actual array comparisons to determine if the rule should cause the processor
		/// to execute.
		/// </summary>
		/// <param name="processTargets">List of targets that, if matched, prevent the Processor from executing.</param>
		/// <param name="ignoreTargets">List of targets that, if matched, cause the Processor to execute.</param>
		/// <param name="candidate">The string to compare.</param>
		/// <returns><c>true</c> if there is one process match or if there is not an ignore match.</returns>
		private static bool PassesRuleTest(ICollection<string> processTargets, ICollection<string> ignoreTargets, string candidate)
		{
			if (string.IsNullOrEmpty(candidate))
			{
				return true;
			}

			if (!HasRuleConfigured(processTargets, ignoreTargets))
			{
				return true;
			}

			return IncludedInTargets(processTargets, candidate) || !IncludedInTargets(ignoreTargets, candidate);
		}

		/// <summary>
		/// Performs a binary search of the target array to determine if the candidate is within.
		/// </summary>
		/// <param name="targets">The List of targets to match.</param>
		/// <param name="candidate">The string to compare.</param>
		/// <returns><c>true</c> if there is a match.</returns>
		private static bool IncludedInTargets(IEnumerable<string> targets, string candidate)
		{
			if (targets == null)
			{
				return false;
			}

			foreach (var target in targets)
			{
				if (candidate.StartsWith(target.Trim(), StringComparison.InvariantCultureIgnoreCase))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Tests for the presents of non-null, non-empty arrays for a given rule.
		/// </summary>
		/// <param name="process">The "process" array.</param>
		/// <param name="ignore">The "ignore" array.</param>
		/// <returns><c>true</c> if either array is non-null and has a length &gt; 0.</returns>
		private static bool HasRuleConfigured(ICollection<string> process, ICollection<string> ignore)
		{
			if (process != null && process.Count > 0)
			{
				return true;
			}

			if (ignore != null && ignore.Count > 0)
			{
				return true;
			}

			return false;
		}
		#endregion

		#region Helpers
		/// <summary>
		/// Given a comma-delimited string, separates it into its component parts.
		/// </summary>
		/// <param name="values">The string to parse.</param>
		/// <returns>A list of values.</returns>
		private static ICollection<string> ConvertToList(string values)
		{
			var output = new List<string>();

			if (!string.IsNullOrEmpty(values))
			{
				var separated = values.Split(',');
				output.AddRange(separated);
			}

			return output;
		}
		#endregion
	}
}