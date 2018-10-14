using System.Collections.Generic;
using Sitecore.Data;

namespace Constellation.Foundation.ModelMapping
{
	public class MappingPlan
	{
		#region Fields
		protected readonly ICollection<ID> FieldIDs;
		#endregion


		public MappingPlan()
		{
			FieldIDs = new List<ID>();
		}


		#region Properties
		public string TypeFullName { get; set; }
		#endregion

		#region Methods
		public void AddField(ID id)
		{
			if (!FieldIDs.Contains(id))
			{
				FieldIDs.Add(id);
			}
		}

		public ICollection<ID> GetFieldIDs()
		{
			return FieldIDs;
		}
		#endregion
	}
}
