using System.Web.Mvc;
using Constellation.Feature.PageAnalyticsScripts.Models;
using Constellation.Foundation.ModelMapping;
using Sitecore.Mvc.Presentation;

namespace Constellation.Feature.PageAnalyticsScripts.Controllers
{
	public abstract class PageAnalyticsScriptsController : Controller
	{
		public ActionResult Index()
		{
			if (!Sitecore.Context.PageMode.IsNormal)
			{
				return Content("<!-- Page Analytics will appear here -->");
			}

			var item = RenderingContext.Current.Rendering.Item;

			var model = item.MapToNew<PageAnalyticsScriptsModel>();

			return Content(GetScriptToRender(model));
		}

		/// <summary>
		/// Use this method to specify which field on the Model to render.
		/// </summary>
		/// <param name="model">The model containing relevant scripts from the Datasource</param>
		/// <returns>The string to render. It could be empty.</returns>
		protected abstract string GetScriptToRender(PageAnalyticsScriptsModel model);
	}
}
