using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using System.Diagnostics.CodeAnalysis;

namespace Constellation.Foundation.Globalization
{
	/// <summary>
	/// Extends Sitecore.Globalization.Language
	/// </summary>
	public static class LanguageExtensions
	{
		/// <summary>
		/// Gets the Item that defines the Language.
		/// </summary>
		/// <param name="language">The Language to parse.</param>
		/// <param name="database">The Database to use.</param>
		/// <returns>A sitecore Item representing the Language.</returns>
		public static Item GetLanguageItem(this Language language, Database database)
		{
			// ReSharper disable ConvertPathToId
			return database.GetItem("/sitecore/system/languages/" + language.Name, language);
			// ReSharper restore ConvertPathToId
		}

		/// <summary>
		/// Gets the Item that defines the Language.
		/// </summary>
		/// <param name="language">The Language to parse.</param>
		/// <returns>A sitecore Item representing the Language.</returns>
		public static Item GetLanguageItem(this Language language)
		{
			return GetLanguageItem(language, global::Sitecore.Context.Database);
		}

		/// <summary>
		/// Returns the DisplayName of the matching Language Item in the
		/// Language of the Language supplied. ex: pt-PT (Portuguese) should
		/// come back as "Português"
		/// </summary>
		/// <param name="language">The language to parse.</param>
		/// <returns>A string naming the language in its own tongue or the item name.</returns>
		[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "StyleCop doesn't speak ISO or Portuguese.")]
		public static string GetNaturalDisplayName(this Language language)
		{
			return GetNaturalDisplayName(language, global::Sitecore.Context.Database);
		}

		/// <summary>
		/// Returns the DisplayName of the matching Language Item in the
		/// Language of the Language supplied. ex: pt-PT (Portuguese) should
		/// come back as "Português"
		/// </summary>
		/// <param name="language">The language to parse.</param>
		/// <param name="database">The database to use.</param>
		/// <returns>A string naming the language in its own tongue or the item name.</returns>
		[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "StyleCop doesn't speak ISO or Portuguese.")]
		public static string GetNaturalDisplayName(this Language language, Database database)
		{
			var item = GetLanguageItem(language, database);
			return item.DisplayName;
		}
	}
}