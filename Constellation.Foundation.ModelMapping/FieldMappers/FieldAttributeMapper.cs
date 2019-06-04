using System;
using System.Reflection;
using Constellation.Foundation.Data;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	/// <summary>
	/// Base class for mapping an XML Attribute of a Sitecore XML Field to properties on a Model.
	/// </summary>
	public abstract class FieldAttributeMapper : IFieldMapper
	{
		#region properties
		/// <summary>
		/// The Field to extract the Attribute from.
		/// </summary>
		protected Field Field { get; set; }

		/// <summary>
		/// The Property to assign the Attribute to.
		/// </summary>
		protected PropertyInfo Property { get; set; }

		/// <summary>
		/// The Model to which the Property belongs.
		/// </summary>
		protected object Model { get; set; }
		#endregion

		/// <summary>
		/// Attempts to map a specific Attribute of a Sitecore XML Field to a specific Property of the supplied modelInstance.
		/// </summary>
		/// <param name="modelInstance">The model to inspect.</param>
		/// <param name="field">The field to inspect.</param>
		/// <returns>A MappingStatus indicating whether the mapping was successful and other useful performance facts.</returns>
		public virtual FieldMapStatus Map(object modelInstance, Field field)
		{
			Model = modelInstance;
			Field = field;

			var name = GetPropertyName();

			Property = modelInstance.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public);

			if (Property == null)
			{
				return FieldMapStatus.NoProperty;
			}

			try
			{
				var value = GetValueToAssign();

				Property.SetValue(Model, value);

				return FieldMapStatus.Success;
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message, ex, this);
				return FieldMapStatus.Exception;
			}
		}


		/// <summary>
		/// Given the Field Name and the Attribute Name, return the expected name of the Property on the Model?
		/// </summary>
		/// <returns>The Name of the property to look for.</returns>
		protected virtual string GetPropertyName()
		{
			return Field.Name.AsPropertyName();
		}

		/// <summary>
		/// Extract the value from the Field's Attribute ready for assignment to the Property.
		/// </summary>
		/// <returns></returns>
		protected abstract object GetValueToAssign();
	}
}
