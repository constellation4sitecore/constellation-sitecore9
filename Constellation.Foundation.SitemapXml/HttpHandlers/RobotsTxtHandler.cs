using Sitecore.Sites;
using System;
using System.Text;
using System.Web;

namespace Constellation.Foundation.SitemapXml.HttpHandlers
{
	/// <inheritdoc />
	/// <summary>
	/// Handles requests for robots.txt and response with a reference to the hostname-specific
	/// sitemap.xml file.
	/// </summary>
	public class RobotsTxtHandler : IHttpHandler
	{
		#region IHttpHandler Members
		/// <inheritdoc />
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

			var builder = new StringBuilder();

			builder.AppendLine("User-agent: *");

			var globalRules = RobotsTxtConfiguration.Current.GlobalRules;
			var defaultRules = RobotsTxtConfiguration.Current.DefaultRules;
			var siteRules = defaultRules; // in case the site does not have its own custom rules.

			if (RobotsTxtConfiguration.Current.SiteRules.ContainsKey(site.Name))
			{
				siteRules = RobotsTxtConfiguration.Current.SiteRules[site.Name]; // load the site's custom rules.
			}

			var includeSitemap = true;

			foreach (var rule in globalRules)
			{
				switch (rule.Permission)
				{
					case RobotsTxtRule.RulePermission.Allow:
						builder.AppendLine($"Allow: {rule.Path}");
						break;

					case RobotsTxtRule.RulePermission.Deny:
						builder.AppendLine($"Disallow: {rule.Path}");

						if (rule.Path.Equals("/", StringComparison.InvariantCultureIgnoreCase))
						{
							includeSitemap = false;
						}

						break;
				}
			}

			foreach (var rule in siteRules)
			{
				switch (rule.Permission)
				{
					case RobotsTxtRule.RulePermission.Allow:
						builder.AppendLine($"Allow: {rule.Path}");
						break;

					case RobotsTxtRule.RulePermission.Deny:
						builder.AppendLine($"Disallow: {rule.Path}");

						if (rule.Path.Equals("/", StringComparison.InvariantCultureIgnoreCase))
						{
							includeSitemap = false;
						}

						break;
				}
			}

			if (includeSitemap)
			{
				builder.AppendLine();
				builder.AppendLine($"Sitemap: {context.Request.Url.GetLeftPart(System.UriPartial.Authority)}/sitemap.xml");
			}

			context.Response.Clear();
			context.Response.ContentType = "text";
			HttpContext.Current.Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate"); // HTTP 1.1.
			HttpContext.Current.Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.0.
			HttpContext.Current.Response.AppendHeader("Expires", "0"); // Proxies.
			context.Response.Write(builder.ToString());
			context.Response.End();
		}
		#endregion
	}
}
