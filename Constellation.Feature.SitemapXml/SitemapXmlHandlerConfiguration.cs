using System;

namespace Constellation.Feature.SitemapXml
{
	/// <summary>
	/// The sitemap xml handler configuration.
	/// </summary>
	public class SitemapXmlHandlerConfiguration
	{
		#region Locals

		private static volatile SitemapXmlHandlerConfiguration _current;

		private static object _lockObject = new object();
		#endregion

		#region Properties

		public static SitemapXmlHandlerConfiguration Current
		{
			get
			{
				if (_current == null)
				{
					lock (_lockObject)
					{
						if (_current == null)
						{
							_current = CreateNewConfiguration();
						}
					}
				}

				return _current;
			}
		}


		public Type DefaultCrawlerType { get; private set; }

		public Type DefaultSitemapNodeType { get; private set; }

		public int DefaultCacheTimeout { get; private set; }
		#endregion


		private static SitemapXmlHandlerConfiguration CreateNewConfiguration()
		{
			var output = new SitemapXmlHandlerConfiguration();


			var crawlerNode = Sitecore.Configuration.Factory.GetConfigNode("constellation/sitemapXml/defaultCrawler");

			output.DefaultCrawlerType = crawlerNode?.Attributes?["type"]?.Value == null ? typeof(DefaultCrawler) : Type.GetType(crawlerNode.Attributes["type"].Value);

			var siteMapNode = Sitecore.Configuration.Factory.GetConfigNode("constellation/sitemapXml/defaultSitemapNode");

			output.DefaultSitemapNodeType = siteMapNode?.Attributes?["type"]?.Value == null
				? typeof(DefaultSitemapNode)
				: Type.GetType(siteMapNode.Attributes["type"].Value);

			var cacheTimeoutNode = Sitecore.Configuration.Factory.GetConfigNode("constellation/sitemapXml/defaultCacheTimeout");
			output.DefaultCacheTimeout = cacheTimeoutNode?.Attributes?["minutes"]?.Value == null
				? 45
				: int.Parse(cacheTimeoutNode.Attributes["minutes"].Value);

			return output;
		}
	}
}
