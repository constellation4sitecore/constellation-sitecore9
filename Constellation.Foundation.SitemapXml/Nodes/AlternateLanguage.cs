namespace Constellation.Foundation.SitemapXml.Nodes
{
	/// <summary>
	/// Details about a language specific alternate link
	/// </summary>
	public class AlternateLanguage
	{
		/// <summary>
		/// the absolute URL for the given language variant
		/// </summary>
		public string Href { get; set; }

		/// <summary>
		/// The ISO language code for the given language variant
		/// </summary>
		public string HrefLang { get; set; }
	}
}
