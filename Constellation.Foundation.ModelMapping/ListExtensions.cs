using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Constellation.Foundation.ModelMapping
{
	public static class ListExtensions
	{
		public static ICollection<T> ToCollectionOf<T>(this ICollection<Item> items)
			where T : class, new()
		{
			return ModelMapper.MapToCollectionOf<T>(items);
		}
	}
}
