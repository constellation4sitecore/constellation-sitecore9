using Constellation.Feature.Redirects.Models;
using Sitecore.ContentSearch;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;
using System;
using System.Web;

namespace Constellation.Feature.Redirects.Pipelines.HttpRequest
{
	/// <summary>
	/// Custom Sitecore Http Request Pipeline step that looks up the current request URL in the redirect manager.
	/// </summary>
	public class RedirectResolver : Constellation.Foundation.Contexts.Pipelines.ContextSensitiveHttpRequestProcessor
	{
		/// <summary>
		/// The code to execute if this Pipeline processor is in a valid context.
		/// </summary>
		/// <param name="args">The Request Args</param>
		protected override void Execute(HttpRequestArgs args)
		{
			if (Sitecore.Context.Item != null)
			{
				Log.Debug("Constellation RedirectResolver: Found Context Item. Exiting.");
				return;
			}

			if (Sitecore.Context.Database == null)
			{
				Log.Debug("Constellation RedirectResolver: No Context Database. Exiting.");
			}

			if (Sitecore.Context.Site == null)
			{
				Log.Debug("Constellation RedirectResolver: No Context Site. Exiting.");
			}

			if (args.PermissionDenied)
			{
				Log.Debug("Constellation RedirectResolver: Request was for an Item the current user cannot access, no redirect attempt should be made in this case.");
				return;
			}

			Uri url = new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Query));
			var localPath = url.LocalPath;

			Log.Debug($"Constellation RedirectResolver processing: '{url}'", this);

			var redirect = FindRedirectRecordFor(localPath);

			if (string.IsNullOrEmpty(redirect?.NewUrl))
			{
				Log.Debug($"Constellation RedirectResolver: No redirect for {localPath}", this);
				return;
			}

			var newUrl = GetAbsoluteNewUrl(redirect);

			if (redirect.IsPermanent)
			{
				Log.Info($"Constellation RedirectResolver: permanently redirecting from unresolved {localPath} to {redirect.NewUrl}", this);
				HttpContext.Current.Response.RedirectPermanent(newUrl, true);

			}
			else
			{
				Log.Info($"Constellation RedirectResolver: redirecting from unresolved {localPath} to {redirect.NewUrl}", this);
				HttpContext.Current.Response.Redirect(newUrl, true);
			}
		}

		/// <summary>
		/// The code that should execute if this pipeline processor is not in a valid context.
		/// </summary>
		/// <param name="args"></param>
		protected override void Defer(HttpRequestArgs args)
		{
			Log.Debug(
				$"Constellation RedirectResolver: Execution deferred for request {HttpContext.Current.Request.Url.OriginalString}",
				this);
		}

		private string GetAbsoluteNewUrl(MarketingRedirect redirect)
		{
			if (redirect.NewUrl.StartsWith("http"))
			{
				return redirect.NewUrl;
			}

			var authority = GetNewUrlAuthority();

			var path = redirect.NewUrl;

			if (path.StartsWith("/"))
			{
				path = path.Substring(1); // remove the leading "/" since prefix will provide one.
			}

			return authority + path;
		}

		private string GetNewUrlAuthority()
		{
			string scheme = "http";

			if (!string.IsNullOrEmpty(Sitecore.Context.Site.SiteInfo.Scheme))
			{
				scheme = Sitecore.Context.Site.SiteInfo.Scheme;
			}

			string hostname = Sitecore.Context.Site.HostName;

			if (!string.IsNullOrEmpty(Sitecore.Context.Site.TargetHostName))
			{
				hostname = Sitecore.Context.Site.TargetHostName;
			}

			string virtualFolder = Sitecore.Context.Site.VirtualFolder;

			return $"{scheme}://{hostname}{virtualFolder}";
		}


		/// <summary>
		/// Retrieves a redirect record for the provided local path based on the context site.
		/// </summary>
		private MarketingRedirect FindRedirectRecordFor(string oldLocalPath)
		{
			var siteRoot = Sitecore.Context.Database.GetItem(Sitecore.Context.Site.StartPath, Sitecore.Context.Language);
			var indexable = new SitecoreIndexableItem(siteRoot);
			var repository = new Repository(Sitecore.Context.Database, ContentSearchManager.GetIndex(indexable));
			return repository.GetNewUrl(Sitecore.Context.Site.SiteInfo, oldLocalPath);
		}
	}
}
