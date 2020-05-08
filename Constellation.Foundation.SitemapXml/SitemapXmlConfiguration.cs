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

		/// <summary>
		/// Access the current configuration here.
		/// </summary>
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

		/// <summary>
		/// Sites that should not have a sitemap.xml file.
		/// </summary>
		public ICollection<string> SitesToIgnore { get; private set; }

		/// <summary>
		/// The crawlers to use if a site does not have crawlers specified.
		/// </summary>
		public ICollection<Type> DefaultCrawlers { get; private set; }

		/// <summary>
		/// The crawlers defined for each site, keyed by site name.
		/// </summary>
		public IDictionary<string, ICollection<Type>> SiteCrawlers { get; set; }

		/// <summary>
		/// Whether generated sitemap.xml documents should be cached.
		/// </summary>
		public bool CacheEnabled { get; private set; }

		/// <summary>
		/// Whether the (obsolete) change frequency attribute should be written to nodes in the sitemap.xml document.
		/// </summary>
		public bool IncludeChangeFrequency { get; private set; }

		/// <summary>
		/// Whether the (obsolete) last modified attribute should be written to nodes in the sitemap.xml document.
		/// </summary>
		public bool IncludeLastMod { get; private set; }

		/// <summary>
		/// Whether the (obsolete) priority attribute should be written to nodes in the sitemap.xml document.
		/// </summary>
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

		/// <summary>
		/// Returns a collection of crawler Types that should be executed for the provided site name.
		/// </summary>
		/// <param name="siteName">the name of the site.</param>
		/// <returns>collection of crawler Types</returns>
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

			output.IncludeChangeFrequency = rootNode?.Attributes?["includeChangeFrequency"]?.Value != null && bool.Parse(rootNode.Attributes["includeChangeFrequency"].Value);

			output.IncludeLastMod = rootNode?.Attributes?["includeLastMod"]?.Value != null && bool.Parse(rootNode.Attributes["includeLastMod"].Value);

			output.IncludePriority = rootNode?.Attributes?["includePriority"]?.Value != null && bool.Parse(rootNode.Attributes["includePriority"].Value);

			var defaultCrawlersNode = Sitecore.Configuration.Factory.GetConfigNode("constellation/sitemapXml/crawlers/defaultCrawlers");

			if (defaultCrawlersNode == null || !defaultCrawlersNode.HasChildNodes)
			{
				var ex = new Exception("No default crawlers configured.");

				Log.Error("Constellation.Foundation.SitemapXml configuration error:", ex, output);
				throw ex;
			}

			foreach (XmlNode crawlerNode in defaultCrawlersNode.ChildNodes)
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

