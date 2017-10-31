namespace Constellation.Foundation
{

	using Constellation.Foundation.Items;
	using Sitecore.Data.Items;
	using Sitecore.Globalization;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// Allows for the conversion of native Sitecore Item arrays into
	/// strongly typed versions for easier programming.
	/// </summary>
	public static class ItemCollectionExtensions
	{
		/// <summary>
		/// Given an array of Items, returns an enumerable list of
		/// strongly typed items.
		/// </summary>
		/// <param name="items">
		/// The original array.
		/// </param>
		/// <returns>
		/// An enumerable list of strongly typed items.
		/// </returns>
		public static IEnumerable<IStandardTemplate> AsStronglyTypedCollection(this IEnumerable<Item> items)
		{
			return items.Select(i => i.AsStronglyTyped()).Where(x => x != null).ToArray();
		}

		/// <summary>
		/// Given an array of Items, returns an enumerable list of
		/// strongly typed items. This extension ensures that the returned
		/// items are in a specific language.
		/// </summary>
		/// <param name="items">
		/// The original array.
		/// </param>
		/// <param name="language">
		/// The language of the strongly typed items in the returned list.
		/// </param>
		/// <returns>
		/// An enumerable list of strongly typed items. Some Items may be null or empty.
		/// </returns>
		public static IEnumerable<IStandardTemplate> AsStronglyTypedCollection(this IEnumerable<Item> items, Language language)
		{
			return items.Select(i => i.AsStronglyTyped(language)).Where(x => x != null).ToArray();
		}

		/// <summary>
		/// Given an array of Items, returns an enumerable list of
		/// strongly typed items.
		/// </summary>
		/// <typeparam name="TItem">
		/// The strongly-typed class or interface to convert the supplied Items to.
		/// </typeparam>
		/// <param name="items">
		/// The original array.
		/// </param>
		/// <returns>
		/// An enumerable list of strongly typed items in the supplied type parameter.
		/// </returns>
		public static IEnumerable<TItem> As<TItem>(this IEnumerable<Item> items)
			where TItem : class, IStandardTemplate
		{
			return items.Select(i => i.As<TItem>()).Where(x => x != null).ToArray();
		}

		/// <summary>
		/// Given an array of Items, returns an enumerable list of
		/// strongly typed items.
		/// </summary>
		/// <typeparam name="TItem">
		/// The strongly-typed class or interface to convert the supplied Items to.
		/// </typeparam>
		/// <param name="items">
		/// The original array.
		/// </param>
		/// <param name="language">
		/// The language of the strongly typed items in the returned list.
		/// </param>
		/// <returns>
		/// An enumerable list of strongly typed items in the supplied type parameter. Some Items may be null or empty.
		/// </returns>
		public static IEnumerable<TItem> As<TItem>(this IEnumerable<Item> items, Language language)
			where TItem : class, IStandardTemplate
		{
			return items.Select(i => i.As<TItem>(language)).Where(x => x != null).ToArray();
		}
	}
}
