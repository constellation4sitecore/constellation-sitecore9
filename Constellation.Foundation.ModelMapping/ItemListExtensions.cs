using System;
using System.Collections.Generic;
using Sitecore.Collections;
using Sitecore.Data.Items;

namespace Constellation.Foundation.ModelMapping
{
	/// <summary>
	/// Syntax candy for converting various Item list types to model lists.
	/// </summary>
	public static class ItemListExtensions
	{
		/// <summary>
		/// Calls IModelMapper.MapToCollectionOf() on behalf of the Collection.
		/// </summary>
		/// <param name="list">The Collection to process.</param>
		/// <typeparam name="T">The model type for the outbound collection.</typeparam>
		/// <returns>A collection of "T"</returns>
		[Obsolete]
		public static ICollection<T> ToCollectionOf<T>(this ICollection<Item> list)
		where T : class, new()
		{
			return MappingContext.Current.MapToCollectionOf<T>(list);
		}

		/// <summary>
		/// Calls IModelMapper.MapToCollectionOf() on behalf of the ChildList.
		/// </summary>
		/// <param name="list">The Collection to process.</param>
		/// <typeparam name="T">The model type for the outbound collection.</typeparam>
		/// <returns>A collection of "T"</returns>
		[Obsolete]
		public static ICollection<T> ToCollectionOf<T>(this ChildList list)
			where T : class, new()
		{
			return MappingContext.Current.MapToCollectionOf<T>(list.InnerChildren);
		}

		/// <summary>
		/// Calls IModelMapper.MapToEnumerableOf() on behalf of the Enumerable.
		/// </summary>
		/// <param name="list">The Enumerable to process.</param>
		/// <typeparam name="T">The model type for the outbound collection.</typeparam>
		/// <returns>An enumerable of "T"</returns>
		[Obsolete]
		public static IEnumerable<T> ToEnumerableOf<T>(this IEnumerable<Item> list)
		where T : class, new()
		{
			return MappingContext.Current.MapToEnumerableOf<T>(list);
		}

		/// <summary>
		/// Calls IModelMapper.MapToEnumerableOf() on behalf of the ChildList.
		/// </summary>
		/// <param name="list">The Enumerable to process.</param>
		/// <typeparam name="T">The model type for the outbound collection.</typeparam>
		/// <returns>An enumerable of "T"</returns>
		[Obsolete]
		public static IEnumerable<T> ToEnumerableOf<T>(this ChildList list)
		where T : class, new()
		{
			return MappingContext.Current.MapToEnumerableOf<T>(list);
		}
	}
}
