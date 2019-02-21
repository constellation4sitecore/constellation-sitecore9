using System;
using System.Net;
using Constellation.Feature.Redirects.Models;
using Sitecore.Diagnostics;

namespace Constellation.Feature.Redirects
{
	/// <summary>
	/// Utility for getting the Http Status of a given Marketing Redirect's target.
	/// </summary>
	public class LinkVerifier
	{
		/// <summary>
		/// Retrieves the Http response status of the Url represented in the Marketing Redirect's NewUrl field.
		/// </summary>
		/// <param name="redirect">The Redirect to inspect.</param>
		/// <returns>The result of the Http request.</returns>
		public Status CheckLink(MarketingRedirect redirect)
		{
			var status = new Status();
			var url = GetAbsoluteNewUrl(redirect);

			try
			{
				var request = (HttpWebRequest)WebRequest.Create(url);
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();


				switch (response.StatusCode)
				{
					case HttpStatusCode.OK:
						status.Successful = true;
						break;
					case HttpStatusCode.NotFound:
						status.Message = "Destination URL was not found. If the destination is a Sitecore Item, ensure it is published.";
						break;
					case HttpStatusCode.InternalServerError:
						status.Message = "Destination URL generated a server error. Please investigate.";
						break;
					case HttpStatusCode.Found:
						status.Message = "Destination URL is itself a redirect. You should choose a different destination URL.";
						break;
					case HttpStatusCode.Moved:
						status.Message = "Destination URL is itself a permanent redirect. You should choose a different destination URL.";
						break;
					default:
						status.Message = "Destination URL received an error code from the server. Please investigate.";
						break;
				}
			}
			catch (Exception ex)
			{
				Log.Error($"Constellation.Feature.Redirects.LinkVerifier error while verifying the following destination url: {url}", ex, typeof(LinkVerifier));
				status.Message = "The Destination URL provided is invalid. Please ensure it is a correctly formed URL.";
			}

			return status;
		}

		private static string GetAbsoluteNewUrl(MarketingRedirect redirect)
		{
			if (redirect.NewUrl.StartsWith("http"))
			{
				return redirect.NewUrl;
			}

			var authority = GetNewUrlAuthority(redirect.SiteName);

			var path = redirect.NewUrl;

			if (path.StartsWith("/"))
			{
				path = path.Substring(1); // remove the leading "/" since prefix will provide one.
			}

			return authority + path;
		}

		private static string GetNewUrlAuthority(string siteName)
		{
			var site = Sitecore.Configuration.Factory.GetSite(siteName);

			string scheme = "http";

			if (!string.IsNullOrEmpty(site.SiteInfo.Scheme))
			{
				scheme = site.SiteInfo.Scheme;
			}

			string hostname = site.HostName;

			if (!string.IsNullOrEmpty(site.TargetHostName))
			{
				hostname = site.TargetHostName;
			}

			string virtualFolder = site.VirtualFolder;

			return $"{scheme}://{hostname}{virtualFolder}";
		}

		/// <summary>
		/// The result of an Http Request, including a user-friendly response.
		/// </summary>
		public class Status
		{
			/// <summary>
			/// True if the Http Response provided was "200"
			/// </summary>
			public bool Successful { get; set; }

			/// <summary>
			/// If the Http Response was not 200, contains text that explains the nature of the response.
			/// </summary>
			public string Message { get; set; }
		}
	}
}
