namespace Constellation.Feature.PageTagging.Models
{
	/// <summary>
	/// A ViewModel with information that can be used to generate a meta:robots tag.
	/// </summary>
	public class PageSearchEngineDirectives
	{
		/// <summary>
		/// Gets or sets a value indicating whether the meta:robots tag should include an "index" or "noindex" value.
		/// </summary>
		public bool SearchEngineIndexesPage { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the meta:robots tag should include a "follow" or "nofollow" value.
		/// </summary>
		public bool SearchEngineFollowsLinks { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the meta:robots tag should include a "noimageindex" value.
		/// </summary>
		public bool SearchEngineIndexesImages { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the meta:robots tag should include a "noarchive" and "nocache" value.
		/// </summary>
		public bool SearchEngineCanCachePage { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the meta:robots tag should include a "nosnippet" value.
		/// </summary>
		public bool SearchEngineCanSnippetPage { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the meta:robots tag should include a "noodp" value.
		/// </summary>
		// ReSharper disable once InconsistentNaming
		public bool AllowODPSnippet { get; set; }
	}
}
