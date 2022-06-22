using Constellation.Foundation.Globalization;
using Constellation.Foundation.SitemapXml.Nodes;
using Sitecore.Diagnostics;
using Sitecore.Web;
using System.Globalization;
using System.Xml;


namespace Constellation.Foundation.SitemapXml
{
	/// <summary>
	/// Handles the creation of a sitemap.xml document based upon the current request parameters.
	/// </summary>
	public class SitemapGenerator
	{
		/// <summary>
		/// The XML Site Map to return in the response.
		/// </summary>
		protected XmlDocument Document { get; private set; }

		/// <summary>
		/// Gets the SiteInfo whose sitemap is being generated.
		/// </summary>
		protected SiteInfo Site { get; private set; }

		/// <summary>
		/// Specifies whether to include Language variants for each node in the Sitemap.xml 
		/// </summary>
		protected bool WithLanguageVariants { get; private set; }


		/// <summary>
		/// Creates a new generator instance for generating a Sitemap.Xml document
		/// for the provided site.
		/// </summary>
		/// <param name="site">The Site to crawl to generate a sitemap.</param>
		public SitemapGenerator(SiteInfo site)
		{
			Assert.ArgumentNotNull(site, "site");
			Site = site;

			if (site.SupportedLanguages().Count > 1)
			{
				WithLanguageVariants = true;
			}

			Document = InitializeDocument(WithLanguageVariants);
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
			url.AppendChild(locElement);

			if (WithLanguageVariants)
			{
				foreach (var variant in node.AlternateLanguages)
				{
					var xhtmlElement = Document.CreateElement("xhtml:link");
					var relAlternate = Document.CreateAttribute("rel");
					relAlternate.Value = "alternate";
					xhtmlElement.Attributes.Append(relAlternate);

					var hrefLang = Document.CreateAttribute("hreflang");
					hrefLang.Value = variant.HrefLang;
					xhtmlElement.Attributes.Append(hrefLang);

					var href = Document.CreateAttribute("href");
					href.Value = variant.Href;
					xhtmlElement.Attributes.Append(hrefLang);

					url.AppendChild(xhtmlElement);
				}
			}

			if (SitemapXmlConfiguration.Current.IncludeLastMod)
			{
				var lastModElement = Document.CreateElement("lastmod");
				lastModElement.InnerText = node.LastModified.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
				url.AppendChild(lastModElement);
			}

			if (SitemapXmlConfiguration.Current.IncludeChangeFrequency)
			{
				var changeFreqElement = Document.CreateElement("changefreq");
				changeFreqElement.InnerText = node.ChangeFrequency.ToString().ToLower(CultureInfo.InvariantCulture);
				url.AppendChild(changeFreqElement);
			}

			if (SitemapXmlConfiguration.Current.IncludePriority)
			{
				var priorityElement = Document.CreateElement("priority");
				priorityElement.InnerText = node.Priority.ToString(CultureInfo.InvariantCulture);
				url.AppendChild(priorityElement);
			}

			// ReSharper disable PossibleNullReferenceException
			Document.DocumentElement.AppendChild(url);
			// ReSharper restore PossibleNullReferenceException
		}

		/// <summary>
		/// Creates a new instance of XmlDocument with the appropriate declaration and root node.
		/// </summary>
		/// <returns>A new empty sitemap.xml document.</returns>
		private static XmlDocument InitializeDocument(bool withLanguageVariants)
		{
			var doc = new XmlDocument();
			var declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", string.Empty);
			doc.AppendChild(declaration);

			var urlset = doc.CreateElement("urlset");
			doc.AppendChild(urlset);

			var xmlns = doc.CreateAttribute("xmlns");
			xmlns.Value = "http://www.sitemaps.org/schemas/sitemap/0.9";
			urlset.Attributes.Append(xmlns);

			if (withLanguageVariants)
			{
				var xhtml = doc.CreateAttribute("xmlns:xhtml");
				xhtml.Value = "http://www.w3.org/1999/xhtml";
				urlset.Attributes.Append(xhtml);
			}

			return doc;
		}
		#endregion
	}
}
