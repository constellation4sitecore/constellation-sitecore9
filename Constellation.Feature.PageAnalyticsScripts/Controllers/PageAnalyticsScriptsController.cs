using System.Web.Mvc;
using Constellation.Feature.PageAnalyticsScripts.Models;
using Constellation.Foundation.ModelMapping;
using Sitecore.Mvc.Presentation;

namespace Constellation.Feature.PageAnalyticsScripts.Controllers
{
	public abstract class PageAnalyticsScriptsController : Controller
	{
		#region Constructor

		protected PageAnalyticsScriptsController(IModelMapper modelMapper)
		{
			ModelMapper = modelMapper;
		}
		#endregion

		#region Properties
		protected IModelMapper ModelMapper { get; }
		#endregion

		public ActionResult Index()
		{
			if (!Sitecore.Context.PageMode.IsNormal)
			{
				return Content("<!-- Page Analytics will appear here -->");
			}

			var item = RenderingContext.Current.Rendering.Item;

			var model = ModelMapper.MapItemToNew<PageAnalyticsScriptsModel>(item);

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
