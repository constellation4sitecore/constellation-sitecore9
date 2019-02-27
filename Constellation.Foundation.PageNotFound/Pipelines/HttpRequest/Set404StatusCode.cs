using System.Net;
using System.Web;
using Constellation.Foundation.Contexts.Pipelines;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;

namespace Constellation.Foundation.PageNotFound.Pipelines.HttpRequest
{
	/// <summary>
	/// A pipeline processor that runs as part of the Sitecore HttpResponse pipeline. If the Context Item is the context
	/// site's designated 404 page, then the 404 response code is added to the Response Header.
	/// </summary>
	public class Set404StatusCode : ContextSensitiveHttpRequestProcessor
	{
		/// <summary>
		/// The code to run if the pipeline context is appropriate for this processor.
		/// </summary>
		/// <param name="args">The pipeline args</param>
		protected override void Execute(HttpRequestArgs args)
		{
			if (args.Aborted)
			{
				return;
			}

			if (Sitecore.Context.Site == null || Sitecore.Context.Item == null)
			{
				// Nothing we can do here.
				return;
			}

			/*
			 * Please note: This processor must execute for a very specific kind of request, therefore
			 * you cannot invert the decision below, because the request will execute under conditions
			 * it was not designed to handle. (evidence: it disables TDS because it returns 404 for almost
			 * all non-Sitecore requests!)
			 *
			 * This handler should ONLY return 404 if the context Item is the site's Page Not Found Item.
			 */
			if (Sitecore.Context.Item.ID == Sitecore.Context.Site.GetPageNotFoundID())
			{
				HttpContext.Current.Response.TrySkipIisCustomErrors = true;
				HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.NotFound;
				HttpContext.Current.Response.StatusDescription = "Page not found";
			}
		}

		/// <summary>
		/// The code to run if the pipeline context is not appropriate for this processor.
		/// </summary>
		/// <param name="args">The pipeline args</param>
		protected override void Defer(HttpRequestArgs args)
		{
			Log.Debug($"Constellation.Foundation.PageNotFound Set404StatusCode: Deferred for {HttpContext.Current.Request.Url.OriginalString}", this);
		}
	}
}
