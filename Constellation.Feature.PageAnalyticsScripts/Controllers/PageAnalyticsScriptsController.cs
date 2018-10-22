using System.Web.Mvc;
using Constellation.Feature.PageAnalyticsScripts.Models;
using Constellation.Foundation.ModelMapping;
using Sitecore.Mvc.Presentation;

namespace Constellation.Feature.PageAnalyticsScripts.Controllers
{
	/// <summary>
	/// This is the base class for all analytics script controllers.
	/// Assign to a Controller Rendering in Sitecore.
	/// Assumes the Datasource Item inherits from the Page Analytics Scripts template
	/// included in this Package.
	/// </summary>
	public abstract class PageAnalyticsScriptsController : Controller
	{
		#region Constructor
		/// <summary>
		/// Creates a new instance of PageAnalyticsScriptsController
		/// </summary>
		/// <param name="modelMapper">The IModelMapper instance to use for Item to Model mapping, usually supplied through Dependency Injection.</param>
		protected PageAnalyticsScriptsController(IModelMapper modelMapper)
		{
			ModelMapper = modelMapper;
		}
		#endregion

		#region Properties
		/// <summary>
		/// The IModelMapper to use to convert the supplied Item to a PageAnalyticsScript model instance.
		/// </summary>
		protected IModelMapper ModelMapper { get; }
		#endregion

		/// <summary>
		/// The Action that should be assigned to the Sitecore Controller Rendering. Note that "Index" is the default
		/// value for Controller Actions in Sitecore configuration.
		/// </summary>
		/// <returns>A string containing any script tags found on the supplied datasource Item.</returns>
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
