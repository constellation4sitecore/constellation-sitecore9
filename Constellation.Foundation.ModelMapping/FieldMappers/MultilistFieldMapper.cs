using System;
using System.Collections;
using System.Collections.Generic;
using Sitecore.Data.Fields;

namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	public class MultilistFieldMapper : FieldMapper<IEnumerable>
	{
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
