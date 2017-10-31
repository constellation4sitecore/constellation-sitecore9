namespace Constellation.Foundation.Items
{
	using Sitecore.Data.Items;

	/// <summary>
	/// Adds Constellation.Foundation.Items support to Sitecore Items.
	/// </summary>
	public static class ItemExtensions
	{
		/// <summary>
		/// Converts an Item into a strongly typed Item descending from
		/// IStandardTemplateItem. Will return null if there is no class
		/// defined for the Item's TemplateID.
		/// </summary>
		/// <param name="item">The Item to convert.</param>
		/// <returns>An instance of IStandardTemplateItem or null.</returns>
		public static IStandardTemplate AsStronglyTyped(this Item item)
		{
			return item == null ? null : ItemFactory.GetStronglyTypedItem(item);
		}

		/// <summary>
		/// Converts an Item into a strongly typed Item descending from
		/// IStandardTemplateItem. Will return null if there is no class
		/// defined for the Item's TemplateID.
		/// </summary>
		/// <typeparam name="TItem">The Type of object to create, must descend from IStandardTemplateItem.</typeparam>
		/// <param name="item">The Item to convert.</param>
		/// <returns>An instance of TItem or null if conversion fails.</returns>
		public static TItem As<TItem>(this Item item)
			where TItem : class, IStandardTemplate
		{
			return item?.AsStronglyTyped() as TItem;
		}
	}
}
