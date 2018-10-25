using System;
using Sitecore.Data.Items;

namespace Constellation.Foundation.ModelMapping
{
	/// <summary>
	/// Syntax candy for converting Items to Models.
	/// </summary>
	public static class ItemExtensions
	{
		/// <summary>
		/// For the given Item, produces a new instance of "T" with fields on the Item mapped to properties of "T".
		/// </summary>
		/// <param name="item">The Item to inspect.</param>
		/// <typeparam name="T">The type of Model to instantiate.</typeparam>
		/// <returns>A new instance of "T" with values mapped from Item Fields to object properties where appropriate.</returns>
		[Obsolete]
		public static T MapToNew<T>(this Item item)
			where T : class, new()
		{
			return MappingContext.Current.MapItemToNew<T>(item);
		}
	}
}
