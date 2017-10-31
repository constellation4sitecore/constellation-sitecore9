namespace Constellation.Foundation.Items
{
	using Sitecore.ContentSearch.SearchTypes;
	using Sitecore.Globalization;

	/// <summary>
	/// The base class for custom Plain Old Class Objects generated
	/// by a Constellation.Sitecore.Items type generator.
	/// </summary>
	public static class SearchResultItemExtensions
	{
		/// <summary>
		/// Converts a Poco into a strongly typed Item descending from
		/// IStandardTemplateItem. Will return null if there is no class
		/// defined for the Item's TemplateID.
		/// </summary>
		/// <param name="poco">The SearchResultItem</param>
		/// <typeparam name="TItem">The Type of object to create, must descend from IStandardTemplateItem.</typeparam>
		/// <returns>An instance of TItem or null if conversion fails.</returns>
		public static TItem As<TItem>(this SearchResultItem poco)
			where TItem : class, IStandardTemplate
		{
			var item = poco.GetItem();
			return item.As<TItem>();
		}

		/// <summary>
		/// Converts a Poco into a strongly typed Item descending from
		/// IStandardTemplateItem. Will return null if there is no class
		/// defined for the Item's TemplateID.
		/// </summary>
		/// <param name="poco">The SearchResultItem</param>
		/// <param name="language">
		/// The target language for the item.
		/// </param>
		/// <typeparam name="TItem">
		/// The Type of object to create, must descend from IStandardTemplateItem.
		/// </typeparam>
		/// <returns>
		/// An instance of TItem or null if conversion fails.
		/// </returns>
		public static TItem As<TItem>(this SearchResultItem poco, Language language)
			where TItem : class, IStandardTemplate
		{
			var item = poco.GetItem();
			return item.As<TItem>(language);
		}

		/// <summary>
		/// Converts a Poco into a strongly typed Item descending from
		/// IStandardTemplateItem. Will return null if there is no class
		/// defined for the Item's TemplateID.
		/// </summary>
		/// <param name="poco">The SearchResultItem</param>
		/// <returns>An instance of IStandardTemplateItem or null.</returns>
		public static IStandardTemplate AsStronglyTyped(this SearchResultItem poco)
		{
			var item = poco.GetItem();
			return item.AsStronglyTyped();
		}

		/// <summary>
		/// Converts a Poco into a strongly typed Item descending from
		/// IStandardTemplateItem. Will return null if there is no class
		/// defined for the Item's TemplateID.
		/// </summary>
		/// <param name="poco">The SearchResultItem</param>
		/// <param name="language">
		/// The target language for the item.
		/// </param>
		/// <returns>An instance of IStandardTemplateItem or null.</returns>
		public static IStandardTemplate AsStronglyTyped(this SearchResultItem poco, Language language)
		{
			var item = poco.GetItem();
			return item.AsStronglyTyped(language);
		}
	}
}
