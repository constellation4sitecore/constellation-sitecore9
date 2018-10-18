using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Diagnostics;

namespace Constellation.Foundation.ModelMapping
{
	public class PlanCache
	{
		#region Locals
		private static volatile PlanCache _current;

		private static readonly object LockObject = new object();
		#endregion

		#region Constructors

		protected PlanCache()
		{
			Cache = new SortedDictionary<string, MappingPlan>();
		}
		#endregion

		#region Properties
		protected readonly SortedDictionary<string, MappingPlan> Cache;

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

		public static void AddPlan(MappingPlan plan)
		{
			var key = GetKey(plan);

			if (Current.Cache.ContainsKey(key))
			{
				Log.Debug($"MappingPlan: Plan for class {plan.TypeFullName} and template {plan.TemplateID} already cached. Replacing.", typeof(PlanCache));
				Current.Cache[key] = plan;
				return;
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
