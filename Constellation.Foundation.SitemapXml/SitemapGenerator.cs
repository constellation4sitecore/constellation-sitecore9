using System.Globalization;
using System.Xml;
using Constellation.Foundation.SitemapXml.Nodes;
using Sitecore.Diagnostics;
using Sitecore.Web;

namespace Constellation.Foundation.SitemapXml
{
	/// <summary>
	/// Handles the creation of a sitemap.xml document based upon the current request parameters.
	/// </summary>
	public class SitemapGenerator
	{
		private XmlDocument _doc;

		/// <summary>
		/// The XML Site Map to return in the response.
		/// </summary>
		protected XmlDocument Document
		{
			get
			{
				if (_doc == null)
				{
					_doc = InitializeDocument();
				}

				return _doc;
			}
		}

		/// <summary>
		/// Gets the SiteInfo whose sitemap is being generated.
		/// </summary>
		protected SiteInfo Site { get; private set; }


		/// <summary>
		/// Creates a new generator instance for generating a Sitemap.Xml document
		/// for the provided site.
		/// </summary>
		/// <param name="site">The Site to crawl to generate a sitemap.</param>
		public SitemapGenerator(SiteInfo site)
		{
			Assert.ArgumentNotNull(site, "site");
			Site = site;
		}

		/// <summary>
		/// Creates the sitemap.xml document, ready for streaming to the response.
		/// </summary>
		/// <returns>
		/// True if the sitemap was generated successfully.
		/// </returns>
		public XmlDocument Generate()
		{
			var manager = new CrawlerManager(Site);

			var nodes = manager.InitiateCrawl();

			foreach (var node in nodes)
			{
				if (node.IsValidForInclusionInSitemapXml())
				{
					AppendUrlElement(node);
				}
			}

			return Document;
		}

		#region XML Management
		/// <summary>
		/// Creates a sitemap.xml fragment and appends it to the sitemap.xml document in progress.
		/// </summary>
		/// <param name="node">The node to process.</param>
		protected virtual void AppendUrlElement(ISitemapNode node)
		{
			var url = Document.CreateElement("url");

			var locElement = Document.CreateElement("loc");
			locElement.InnerText = node.Location;

			var lastModElement = Document.CreateElement("lastmod");
			lastModElement.InnerText = node.LastModified.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

			var changeFreqElement = Document.CreateElement("changefreq");
			changeFreqElement.InnerText = node.ChangeFrequency.ToString().ToLower(CultureInfo.InvariantCulture);

			var priorityElement = Document.CreateElement("priority");
			priorityElement.InnerText = node.Priority.ToString(CultureInfo.InvariantCulture);

			url.AppendChild(locElement);
			url.AppendChild(lastModElement);
			url.AppendChild(changeFreqElement);
			url.AppendChild(priorityElement);

			// ReSharper disable PossibleNullReferenceException
			Document.DocumentElement.AppendChild(url);
			// ReSharper restore PossibleNullReferenceException
		}

		/// <summary>
		/// Creates a new instance of XmlDocument with the appropriate declaration and root node.
		/// </summary>
		/// <returns>A new empty sitemap.xml document.</returns>
		private static XmlDocument InitializeDocument()
		{
			var doc = new XmlDocument();
			var declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", string.Empty);
			doc.AppendChild(declaration);

			var urlset = doc.CreateElement("urlset");
			doc.AppendChild(urlset);

			var xmlns = doc.CreateAttribute("xmlns");
			xmlns.Value = "http://www.sitemaps.org/schemas/sitemap/0.9";
			urlset.Attributes.Append(xmlns);

			return doc;
		}
		#endregion
	}
}
