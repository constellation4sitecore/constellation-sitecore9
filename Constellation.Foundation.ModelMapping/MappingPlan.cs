using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Diagnostics;

namespace Constellation.Foundation.ModelMapping
{
	public class MappingPlan
	{
		#region Fields
		protected readonly ICollection<ID> FieldIDs;
		#endregion


		public MappingPlan(string typeFullName, ID templateID)
		{
			TypeFullName = typeFullName;
			TemplateID = templateID;
			FieldIDs = new List<ID>();
		}


		#region Properties
		public string TypeFullName { get; }

		public ID TemplateID { get; }
		#endregion

		#region Methods
		public void AddField(ID id)
		{
			if (!FieldIDs.Contains(id))
			{
				FieldIDs.Add(id);
				Log.Debug($"ModelMapping.MappingPlan: Added {id} to the Mapping Plan for type {TypeFullName} and template {TemplateID}", this);
			}
		}

		public ICollection<ID> GetFieldIDs()
		{
			return FieldIDs;
		}
		#endregion
	}
}
