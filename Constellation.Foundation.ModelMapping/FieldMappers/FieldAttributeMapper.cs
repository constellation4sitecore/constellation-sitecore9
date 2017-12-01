using Constellation.Foundation.Data;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using System;
using System.Reflection;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public abstract class FieldAttributeMapper : IFieldMapper
	{
		#region properties
		protected Field Field { get; set; }

		protected PropertyInfo Property { get; set; }

		protected object Model { get; set; }
		#endregion

		public FieldMapStatus Map(object modelInstance, Field field)
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


		protected virtual string GetPropertyName()
		{
			return Field.Name.AsPropertyName();
		}

		protected abstract object GetValueToAssign();
	}
}
