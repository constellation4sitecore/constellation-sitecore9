using System.Web.Mvc;
using Constellation.Foundation.ModelMapping;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Presentation;


namespace Constellation.Feature.Redirects.Controllers
{
	/// <summary>
	/// An Item Controller that should be assigned to the Page Redirect Item Template
	/// </summary>
	public class PageRedirectController : Controller
	{
		#region Constructor
		/// <summary>
		/// Creates a new instance of Page Redirect Controller.
		/// </summary>
		/// <param name="modelMapper">The ModelMapper instance to use.</param>
		public PageRedirectController(IModelMapper modelMapper)
		{
			ModelMapper = modelMapper;
		}
		#endregion

		#region Properties
		/// <summary>
		/// An instance of ModelMapper
		/// </summary>
		public IModelMapper ModelMapper { get; }
		#endregion

		/// <summary>
		/// the Action for this Controller that performs the redirect.
		/// </summary>
		/// <returns>A redirct result.</returns>
		public ActionResult Index()
		{
			var model = ModelMapper.MapItemToNew<Models.PageRedirect>(RenderingContext.Current.PageContext.Item);

			Log.Debug($"PageRedirectController redirecting request for {model.DisplayName} to {model.RedirectLinkUrl}.", this);

			return new RedirectResult(model.RedirectLinkUrl, model.IsPermanent);
		}
	}
}
