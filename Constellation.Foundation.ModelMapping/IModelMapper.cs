using System.Collections.Generic;
using Sitecore.Data.Items;

namespace Constellation.Foundation.ModelMapping
{
	/// <summary>
	/// An object that will take values from Sitecore Item Fields and map them to properties on
	/// another object designed for this purpose.
	/// </summary>
	public interface IModelMapper
	{
		/// <summary>
		/// For each Item in the collection, will return an instance of "T" with appropriate fields
		/// from the Item mapped to properties of "T".
		/// </summary>
		/// <typeparam name="T">The type of Model to instantiate.</typeparam>
		/// <param name="items">The Items to process.</param>
		/// <returns>A collection of "T" objects, filled with Item field values. The collection may be empty.</returns>
		ICollection<T> MapToCollectionOf<T>(ICollection<Item> items)
			where T : class, new();

		/// <summary>
		/// For each Item in the enumeration, will return an instance of "T" with appropriate fields
		/// from the Item mapped to properties of "T".
		/// </summary>
		/// <param name="items">The Items to process.</param>
		/// <typeparam name="T">The type of Model to instantiate.</typeparam>
		/// <returns>An enumerable of "T" objects, filled with Item field values. The enumerable may be empty.</returns>
		IEnumerable<T> MapToEnumerableOf<T>(IEnumerable<Item> items)
			where T : class, new();

		/// <summary>
		/// For the provided Item, will return a new instance of "T" with appropriate fields from
		/// the Item mapped to properties of "T".
		/// </summary>
		/// <param name="item">The Item to process.</param>
		/// <typeparam name="T">The type of Model to instantiate.</typeparam>
		/// <returns>An instance of "T", filled with Item field values. The output may be null.</returns>
		T MapItemToNew<T>(Item item)
			where T : class, new();

		/// <summary>
		/// For the provided Item, copy Field values to the object provided, where the Field names can be
		/// mapped to Properties of the object.
		/// </summary>
		/// <param name="item">The Item to process.</param>
		/// <param name="model">The object to receive field values.</param>
		void MapTo(Item item, object model);
	}
}
