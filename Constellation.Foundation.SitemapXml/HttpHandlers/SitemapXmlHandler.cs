using Constellation.Foundation.SitemapXml.Repositories;
using Sitecore.Sites;
using System;
using System.Net;
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

			/*
			 * Include the last segment of the server's IP address in a custom diagnostic header.
			 * This can be used to track across multiple CD servers in the event you are getting unusual results
			 * that are not consistent from request to request.
			 */
			IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
			IPAddress ipAddress = ipHostInfo.AddressList[0].MapToIPv4();

			// get the last part only
			var fullIpString = ipAddress.ToString();
			var lastPart = fullIpString.Substring(fullIpString.LastIndexOf(".", fullIpString.Length - 1, StringComparison.Ordinal));

			context.Response.Clear();
			context.Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate"); // HTTP 1.1.
			context.Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.0.
			context.Response.AppendHeader("Expires", "0"); // Proxies.
			context.Response.AppendHeader("X-CD", lastPart);
			context.Response.AppendHeader("X-Constellation-Version", this.GetType().Assembly.GetName().Version.ToString());
			context.Response.ContentType = "text/xml";
			context.Response.Output.Write(document);
			context.Response.End();
		}
		#endregion
	}
}
