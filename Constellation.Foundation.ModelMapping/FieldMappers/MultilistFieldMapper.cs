using System;
using System.Collections;
using System.Collections.Generic;
using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	/// <inheritdoc />
	/// <summary>
	/// Given a Sitecore Field that stores multiple IDs, Load the Target Items and convert them to a Collection of objects as specified by the Model's Property Type.
	/// </summary>
	public class MultilistFieldMapper : FieldMapper<IEnumerable>
	{
		/// <inheritdoc />
		protected override bool PropertyTypeMatches()
		{
			if (!Property.Is<IEnumerable>())
			{
				return false;
			}

			if (!Property.PropertyType.IsGenericType)
			{
				return false;
			}

			var itemType = Property.PropertyType.GetGenericArguments()[0];

			var listType = typeof(List<>).MakeGenericType(itemType);

			var canAssign = Property.PropertyType.IsAssignableFrom(listType);

			return canAssign;
		}

		/// <summary>
		/// Creates an IEnumerable of ViewModels representing the Items in the Field
		/// </summary>
		/// <returns>An Enumerable that is assignable to the Property.</returns>
		protected override IEnumerable ExtractTypedValueFromField()
		{
			var itemType = Property.PropertyType.GetGenericArguments()[0];

			var listType = typeof(List<>).MakeGenericType(itemType);
			var addMethod = listType.GetMethod("Add");


			var list = Activator.CreateInstance(listType);

			var items = ((MultilistField)Field).GetItems();
			if (items == null || items.Length == 0)
			{
				return (IEnumerable)list;
			}

			foreach (var item in items)
			{
				var subModel = Activator.CreateInstance(itemType);

				MappingContext.Current.MapTo(item, subModel);

				// ReSharper disable once PossibleNullReferenceException
				addMethod.Invoke(list, new[] { subModel });
			}

			return (IEnumerable)list;
		}
	}
}
