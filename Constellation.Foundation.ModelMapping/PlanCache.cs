using System.Collections.Generic;
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
		public static MappingPlan GetPlan(string typeFullName)
		{
			if (!_current.Cache.ContainsKey(typeFullName))
			{
				Log.Debug($"MappingPlan: No plan cached for class {typeFullName}.", typeof(PlanCache));
				return null;
			}

			return _current.Cache[typeFullName];
		}

		public static void AddPlan(MappingPlan plan)
		{
			if (_current.Cache.ContainsKey(plan.TypeFullName))
			{
				Log.Debug($"MappingPlan: Plan for class {plan.TypeFullName} already cached. Replacing.", typeof(PlanCache));
				_current.Cache[plan.TypeFullName] = plan;
				return;
			}

			Log.Debug($"ModelMapping.PlanCache: Plan for class {plan.TypeFullName} added.", typeof(PlanCache));
			_current.Cache.Add(plan.TypeFullName, plan);

		}

		private static PlanCache CreateNewPlanCache()
		{
			var output = new PlanCache();

			return output;
		}
		#endregion
	}
}
