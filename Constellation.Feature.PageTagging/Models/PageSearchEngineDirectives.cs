namespace Constellation.Feature.PageTagging.Models
{
	public class PageSearchEngineDirectives
	{
		public bool SearchEngineIndexesPage { get; set; }

		public bool SearchEngineFollowsLinks { get; set; }

		public bool SearchEngineIndexesImages { get; set; }

		public bool SearchEngineCanCachePage { get; set; }

		public bool SearchEngineCanSnippetPage { get; set; }

		// ReSharper disable once InconsistentNaming
		public bool AllowODPSnippet { get; set; }
	}
}
