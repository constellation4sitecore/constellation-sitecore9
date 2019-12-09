using Sitecore.Diagnostics;
using Sitecore.Mvc.Controllers;
using System.Web.Mvc;


namespace Constellation.Feature.Redirects.Controllers
{
	/// <summary>
	/// An Item Controller that should be assigned to the Page Redirect Item Template
	/// </summary>
	public class PageRedirectController : SitecoreController
	{

		/// <summary>
		/// the Action for this Controller that performs the redirect.
		/// </summary>
		/// <returns>A redirct result.</returns>
		public override ActionResult Index()
		{
			var mapper = Constellation.Foundation.ModelMapping.MappingContext.Current;

			var model = mapper.MapItemToNew<Models.PageRedirect>(Sitecore.Context.Item);

			Log.Debug($"PageRedirectController redirecting request for {model.DisplayName} to {model.RedirectLinkUrl}.", this);

			return new RedirectResult(model.RedirectLinkUrl, model.IsPermanent);
		}
	}
}
