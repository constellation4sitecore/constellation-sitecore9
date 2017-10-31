using System.Web;

namespace Constellation.Feature.SitemapXml
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
			var doc = SitemapGenerator.Generate(context.Request, true);

			context.Response.Clear();
			context.Response.ContentType = "text/xml";
			doc.Save(context.Response.OutputStream);
			context.Response.End();
		}
		#endregion
	}
}
