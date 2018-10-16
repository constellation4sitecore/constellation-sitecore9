using System.Collections.Generic;
using Sitecore.Data.Items;

namespace Constellation.Foundation.ModelMapping
{
	public interface IModelMapper
	{
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <returns></returns>
		ICollection<T> MapToCollectionOf<T>(ICollection<Item> items)
			where T : class, new();

		IEnumerable<T> MapToEnumerableOf<T>(IEnumerable<Item> items)
			where T : class, new();

		T MapItemToNew<T>(Item item)
			where T : class, new();

		void MapTo(Item item, object model);
	}
}
