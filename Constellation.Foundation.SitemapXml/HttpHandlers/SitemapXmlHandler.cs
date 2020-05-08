using Constellation.Foundation.SitemapXml.Repositories;
using Sitecore.Sites;
using System.Web;

namespace Constellation.Foundation.SitemapXml.HttpHandlers
{
	/// <inheritdoc />
	/// <summary>
	/// Generates a sitemap.xml file for the sitecore site requested by the hostname component
	/// of the current request.
	/// </summary>
	public class SitemapXmlHandler : IHttpHandler
	{
		#region IHttpHandler Members
		/// <summary>
		/// Gets a value indicating whether this instance of the handler is reusable.
		/// Required by IHttpHandler, not used by developers in this case.
		/// </summary>
		public bool IsReusable
		{
			get { return false; }
		}

		/// <inheritdoc />
		/// <summary>
		/// Required by IHttpHandler, this is called by ASP.NET when an appropriate URL match is found.
		/// </summary>
		/// <param name="context">The current request context.</param>
		public void ProcessRequest(HttpContext context)
		{
			var site = SiteContextFactory.GetSiteContext(context.Request.Url.Host, context.Request.Url.LocalPath, context.Request.Url.Port);

			if (SitemapRepository.IsOnIgnoreList(site))
			{
				context.Response.Clear();
				context.Response.StatusCode = 404;
				context.Response.Status = "Not Found";
				context.Response.End();
			}

			var document = SitemapRepository.GetSitemap(site);

			context.Response.Clear();
			context.Response.ContentType = "text/xml";
			context.Response.Output.Write(document);
			context.Response.End();
		}
		#endregion
	}
}
