using Sitecore.Data;
using Sitecore.Diagnostics;
using System.Collections.Generic;

namespace Constellation.Foundation.ModelMapping
{
	/// <summary>
	/// Stores all Mapping Plans.
	/// </summary>
	public class PlanCache
	{
		#region Locals
		private static volatile PlanCache _current;

		private static readonly object LockObject = new object();
		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new instance of PlanCache
		/// </summary>
		protected PlanCache()
		{
			Cache = new SortedDictionary<string, MappingPlan>();
		}
		#endregion

		#region Properties

		/// <summary>
		/// The currently stored Mapping Plans
		/// </summary>
		public readonly SortedDictionary<string, MappingPlan> Cache;

		/// <summary>
		/// The current PlanCache instance.
		/// </summary>
		public static PlanCache Current
		{
			get
			{
				if (_current == null)
				{
					lock (LockObject)
					{
						if (_current == null)
						{
							_current = CreateNewPlanCache();
						}
					}
				}

				return _current;
			}
		}
		#endregion

		#region Methods

		/// <summary>
		/// Given a Type and an Item Template ID, return the mapping plan that was used to successfully map the Item to the Type previously.
		/// </summary>
		/// <param name="typeFullName">The full name of the Model Type.</param>
		/// <param name="templateID">The ID of the Item's Template.</param>
		/// <returns>The matching MappingPlan or null.</returns>
		public static MappingPlan GetPlan(string typeFullName, ID templateID)
		{
			var key = GetKey(typeFullName, templateID);

			if (!Current.Cache.ContainsKey(key))
			{
				Log.Debug($"MappingPlan: No plan cached for class {typeFullName} and template {templateID}.", typeof(PlanCache));
				return null;
			}

			return Current.Cache[key];
		}

		/// <summary>
		/// Adds a Plan to the cache.
		/// </summary>
		/// <param name="plan">The plan to add.</param>
		public static void AddPlan(MappingPlan plan)
		{
			var key = GetKey(plan);

			if (Current.Cache.ContainsKey(key))
			{
				Log.Warn($"MappingPlan: Plan for class {plan.TypeFullName} and template {plan.TemplateID} already cached. Ignoring.", typeof(PlanCache));
				return;

				//Log.Debug($"MappingPlan: Plan for class {plan.TypeFullName} and template {plan.TemplateID} already cached. Replacing.", typeof(PlanCache));
				//Current.Cache[key] = plan;
				//return;
			}

			Log.Debug($"ModelMapping.PlanCache: Plan for class {plan.TypeFullName} added.", typeof(PlanCache));
			Current.Cache.Add(key, plan);

		}

		private static PlanCache CreateNewPlanCache()
		{
			var output = new PlanCache();

			return output;
		}

		private static string GetKey(MappingPlan plan)
		{
			return GetKey(plan.TypeFullName, plan.TemplateID);
		}

		private static string GetKey(string typeFullName, ID templateID)
		{
			return typeFullName + templateID;
		}


		#endregion
	}
}
