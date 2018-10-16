using System.Collections.Generic;
using Sitecore.Data.Items;

namespace Constellation.Foundation.ModelMapping
{
	public static class ListExtensions
	{
		public static ICollection<T> ToCollectionOf<T>(this ICollection<Item> items)
			where T : class, new()
		{
			return MappingContext.Current.MapToCollectionOf<T>(items);
		}
	}
}
