using System.Collections.Generic;
using Sitecore.Collections;
using Sitecore.Data.Items;

namespace Constellation.Foundation.ModelMapping
{
	public static class ItemListExtensions
	{
		public static ICollection<T> ToCollectionOf<T>(this ICollection<Item> list)
		where T : class, new()
		{
			return ModelMapper.MapToCollectionOf<T>(list);
		}

		public static ICollection<T> ToCollectionOf<T>(this ChildList list)
			where T : class, new()
		{
			return ModelMapper.MapToCollectionOf<T>(list.InnerChildren);
		}

		public static IEnumerable<T> ToEnumerableOf<T>(this IEnumerable<Item> list)
		where T : class, new()
		{
			return ModelMapper.MapToEnumerableOf<T>(list);
		}

		public static IEnumerable<T> ToEnumerableOf<T>(this ChildList list)
		where T : class, new()
		{
			return ModelMapper.MapToEnumerableOf<T>(list);
		}
	}
}
