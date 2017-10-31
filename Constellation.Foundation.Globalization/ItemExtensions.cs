using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace Constellation.Foundation.Globalization
{
	/// <summary>
	/// Set of extensions for interacting with Sitecore items specific to language.
	/// </summary>
	public static class ItemExtensions
	{
		/// <summary>
		/// Verifies that the Item, in its current Language, is not effectively empty.
		/// </summary>
		/// <param name="item">The Item to test.</param>
		/// <returns>True if the Item's Language has one or more Versions.</returns>
		public static bool LanguageVersionIsEmpty(this Item item)
		{
			if (item == null)
			{
				return true;
			}

			var langItem = item.Database.GetItem(item.ID, item.Language);

			return langItem == null || langItem.Versions.Count == 0;
		}

		/// <summary>
		/// Given an Item in a specific language dialect (ex: en-GB), determines if the
		/// dialect has a version, and if not, attempts to find a more generalized language (ex: en).
		/// If there is a more generalized language, returns an item instance in that more generalized
		/// language. The instance may be "empty" and should be checked using the LanguageVersionIsEmpty() extension.
		/// </summary>
		/// <param name="item">The Item to test.</param>
		/// <param name="targetLanguage">The expected language.</param>
		/// <returns>The current language version, or a language version in a more generalized language if available.</returns>
		public static Item GetBestFitLanguageVersion(this Item item, Language targetLanguage)
		{
			var languageSpecificItem = item.Database.GetItem(item.ID, targetLanguage);

			return GetBestFitLanguageVersion(languageSpecificItem);
		}

		/// <summary>
		/// Given an Item in a specific language dialect (ex: en-GB), determines if the
		/// dialect has a version, and if not, attempts to find a more generalized language (ex: en).
		/// If there is a more generalized language, returns an item instance in that more generalized
		/// language. The instance may be "empty" and should be checked using the LanguageVersionIsEmpty() extension.
		/// </summary>
		/// <param name="item">The Item to test.</param>
		/// <returns>The current language version, or a language version in a more generalized language if available.</returns>
		public static Item GetBestFitLanguageVersion(this Item item)
		{
			if (item == null)
			{
				return null;
			}

			if (item.LanguageVersionIsEmpty())
			{
				Language generalizedLanguage;
				if (Language.TryParse(item.Language.Name.Substring(0, 2), out generalizedLanguage))
				{
					return item.Database.GetItem(item.ID, generalizedLanguage);
				}
			}

			return item;
		}
	}
}
