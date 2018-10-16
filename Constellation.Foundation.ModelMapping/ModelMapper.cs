using System.Collections.Generic;
using Sitecore.Data.Items;

namespace Constellation.Foundation.ModelMapping
{
	public class ModelMapper : IModelMapper
	{
		public ICollection<T> MapToCollectionOf<T>(ICollection<Item> items) where T : class, new()
		{
			var list = new List<T>();

			if (items == null) return list;
			foreach (var item in items)
			{
				list.Add(MapItemToNew<T>(item));
			}

			return list;
		}

		public IEnumerable<T> MapToEnumerableOf<T>(IEnumerable<Item> items) where T : class, new()
		{
			var list = new List<T>();

			if (items == null) return list;
			foreach (var item in items)
			{
				list.Add(MapItemToNew<T>(item));
			}

			return list;
		}

		public T MapItemToNew<T>(Item item) where T : class, new()
		{
			if (item == null)
			{
				return null;
			}

			var model = new T();

			MapTo(item, model);

			return model;
		}

		public void MapTo(Item item, object model)
		{
			ModelBuilder.MapItemToModel(item, model);
		}
	}
}
