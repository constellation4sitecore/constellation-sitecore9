using Sitecore.Web;
using System.Collections.Generic;

namespace Constellation.Foundation.Globalization
{
	/// <summary>
	/// Extension Methods for the Sitecore.Web.SiteInfo class.
	/// </summary>
	public static class SiteInfoExtensions
	{
		/// <summary>
		/// Retrieves a collection of supported languages for the SiteInfo instance. The
		/// collection can be populated by adding a custom "supportedLanguages" parameter to
		/// a Sitecore "site" configuration node. Language codes should be separated by commas.
		/// The returned collection includes the official "language" value for the SiteInfo instance,
		/// but not the "contentLanguage" value.
		/// </summary>
		/// <param name="siteInfo">
		/// The SiteInfo to interrogate.
		/// </param>
		/// <returns>
		/// The collection includes the official "language" value for the SiteInfo instance,
		/// but not the "contentLanguage" value.
		/// </returns>
		public static ICollection<string> SupportedLanguages(this SiteInfo siteInfo)
		{
			var primaryLanguageCode = siteInfo.Language;
			var supportedLanguagesAttributeValue = siteInfo.Properties["supportedLanguages"];
			var languageCodes = new List<string>();

			if (!string.IsNullOrEmpty(supportedLanguagesAttributeValue))
			{
				var separated = supportedLanguagesAttributeValue.Split(',');

				foreach (var code in separated)
				{
					if (!string.IsNullOrEmpty(code))
					{
						languageCodes.Add(code);
					}
				}
			}

			if (!languageCodes.Contains(primaryLanguageCode))
			{
				languageCodes.Insert(0, primaryLanguageCode);
			}

			return languageCodes;
		}
	}
}
