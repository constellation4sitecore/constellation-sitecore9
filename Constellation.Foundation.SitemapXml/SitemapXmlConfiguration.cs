using System;
using System.Collections.Generic;
using System.Xml;
using Sitecore.Diagnostics;

namespace Constellation.Foundation.SitemapXml
{
	/// <summary>
	/// The sitemap xml handler configuration.
	/// </summary>
	public class SitemapXmlConfiguration
	{
		#region Locals

		private static volatile SitemapXmlConfiguration _current;

		private static object _lockObject = new object();
		#endregion

		#region Properties

		public static SitemapXmlConfiguration Current
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

		public ICollection<string> SitesToIgnore { get; private set; }

		public ICollection<Type> DefaultCrawlers { get; private set; }

		public IDictionary<string, ICollection<Type>> SiteCrawlers { get; set; }

		public bool CacheEnabled { get; private set; }

		public int DefaultCacheTimeout { get; private set; }

		public bool IncludeChangeFrequency { get; private set; }

		public bool IncludeLastMod { get; private set; }

		public bool IncludePriority { get; private set; }
		#endregion


		#region Constructor
		private SitemapXmlConfiguration()
		{
			SitesToIgnore = new List<string>();
			DefaultCrawlers = new List<Type>();
			SiteCrawlers = new Dictionary<string, ICollection<Type>>();
		}
		#endregion

		#region Methods

		public ICollection<Type> GetCrawlersForSite(string siteName)
		{
			if (SiteCrawlers.ContainsKey(siteName))
			{
				return SiteCrawlers[siteName];
			}

			return DefaultCrawlers;
		}
		#endregion


		private static SitemapXmlConfiguration CreateNewConfiguration()
		{
			var output = new SitemapXmlConfiguration();

			var rootNode = Sitecore.Configuration.Factory.GetConfigNode("constellation/sitemapXml");

			var ignoreAttribute = rootNode?.Attributes?["sitesToIgnore"]?.Value;

			if (!string.IsNullOrEmpty(ignoreAttribute))
			{
				output.SitesToIgnore = ignoreAttribute.Split(',');
			}

			output.CacheEnabled = rootNode?.Attributes?["cacheEnabled"]?.Value != null && bool.Parse(rootNode.Attributes["cacheEnabled"].Value);



			output.DefaultCacheTimeout = rootNode?.Attributes?["defaultCacheTimeout"]?.Value == null
				? 45
				: int.Parse(rootNode.Attributes["defaultCacheTimeout"].Value);

			output.IncludeChangeFrequency = rootNode?.Attributes?["includeChangeFrequency"]?.Value != null && bool.Parse(rootNode.Attributes["includeChangeFrequency"].Value);

			output.IncludeLastMod = rootNode?.Attributes?["includeLastMod"]?.Value != null && bool.Parse(rootNode.Attributes["includeLastMod"].Value);

			output.IncludePriority = rootNode?.Attributes?["includePriority"]?.Value != null && bool.Parse(rootNode.Attributes["includePriority"].Value);

			var defaultNode = Sitecore.Configuration.Factory.GetConfigNode("constellation/sitemapXml/crawlers/defaultCrawlers");

			if (defaultNode == null || !defaultNode.HasChildNodes)
			{
				var ex = new Exception("No default crawlers configured.");

				Log.Error("Constellation.Foundation.SitemapXml configuration error:", ex, output);
				throw ex;
			}

			foreach (XmlNode crawlerNode in defaultNode.ChildNodes)
			{
				var value = crawlerNode?.Attributes?["type"]?.Value;

				if (string.IsNullOrEmpty(value))
				{
					var ex = new Exception("\"crawler\" configuration found with no \"type\" attribute defined.");
					Log.Error("Constellation.Foundation.SitemapXml configuration error:", ex, output);
					throw ex;
				}

				var type = Type.GetType(value);

				output.DefaultCrawlers.Add(type);
			}

			var crawlersnode = Sitecore.Configuration.Factory.GetConfigNode("constellation/sitemapXml/crawlers");

			if (crawlersnode == null || !crawlersnode.HasChildNodes)
			{
				var ex = new Exception("No crawlers configured.");
				Log.Error("Constellation.Foundation.SitemapXml configuration error", ex, output);
				throw ex;
			}

			foreach (XmlNode crawlerNode in crawlersnode.ChildNodes)
			{
				if (crawlerNode != null && crawlerNode.Name == "defaultCrawlers")
				{
					continue;
				}

				var site = crawlerNode?.Attributes?["name"]?.Value;

				if (string.IsNullOrEmpty(site))
				{
					// It's not a valid node
					continue;
				}

				var types = new List<Type>();

				foreach (XmlNode typeNode in crawlerNode.ChildNodes)
				{
					var value = typeNode?.Attributes?["type"]?.Value;

					if (string.IsNullOrEmpty(value))
					{
						var ex = new Exception("\"crawlers/site\" configuration found with no \"type\" attribute defined.");
						Log.Error("Constellation.Foundation.SitemapXml configuration error:", ex, output);
						throw ex;
					}

					var type = Type.GetType(value);

					types.Add(type);
				}

				output.SiteCrawlers.Add(site, types);
			}



			return output;
		}
	}
}

