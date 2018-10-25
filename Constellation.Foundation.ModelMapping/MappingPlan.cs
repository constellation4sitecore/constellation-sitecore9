using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Diagnostics;

namespace Constellation.Foundation.ModelMapping
{
	/// <summary>
	/// Represents a list of fields of a particular template that can be successfully mapped to a particular Type of model.
	/// </summary>
	public class MappingPlan
	{
		#region Fields

		/// <summary>
		/// The IDs of fields that were successfully mapped.
		/// </summary>
		public readonly ICollection<ID> FieldIDs;
		#endregion


		/// <summary>
		/// Creates a new instance of MappingPlan
		/// </summary>
		/// <param name="typeFullName">The Type that is being mapped.</param>
		/// <param name="templateID">The Sitecore Template ID of the item that is being mapped.</param>
		public MappingPlan(string typeFullName, ID templateID)
		{
			TypeFullName = typeFullName;
			TemplateID = templateID;
			FieldIDs = new List<ID>();
		}


		#region Properties
		/// <summary>
		/// The full name of the Type that is being mapped, sufficient to create a new instance of it.
		/// </summary>
		public string TypeFullName { get; }

		/// <summary>
		/// The ID of the Item Template that is being mapped, sufficient to create a new Item of that Template type.
		/// </summary>
		public ID TemplateID { get; }
		#endregion

		#region Methods
		/// <summary>
		/// Indicates that a given Field was successfully mapped, and adds it to the list of Fields to use the next time
		/// the TypeFullName is the target of a mapping.
		/// </summary>
		/// <param name="id"></param>
		public void AddField(ID id)
		{
			if (!FieldIDs.Contains(id))
			{
				FieldIDs.Add(id);
				Log.Debug($"ModelMapping.MappingPlan: Added {id} to the Mapping Plan for type {TypeFullName} and template {TemplateID}", this);
			}
		}

		/// <summary>
		/// Returns the list of Field IDs that were successfully mapped for the Model Type referenced by this Plan.
		/// </summary>
		/// <returns></returns>
		public ICollection<ID> GetFieldIDs()
		{
			return FieldIDs;
		}
		#endregion
	}
}
