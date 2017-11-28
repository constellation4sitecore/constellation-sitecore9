using Sitecore.Data.Items;

namespace Constellation.Foundation.Mvc
{
	using Sitecore.Mvc.Presentation;
	using System.Web.Mvc;

	/// <summary>
	/// A Controller that assumes the location of the matching view based upon the Rendering Item's path.
	/// </summary>
	/// <remarks>
	/// Objects inheriting from this class are only responsible for constructing the Model. The View will be located
	/// automatically based upon the name and content tree position of the Rendering Item that sourced this particular
	/// controller instance. You can control the root location rendering items and views on disk through these Sitecore
	/// configuration settings:
	/// 
	/// Constellation.Foundation.Mvc.ViewRootPath - Establishes the root folder where Views are stored. Default is "~/Views"
	/// Constellation.Foundation.Mvc.RenderingItemPathRoot - Establishes the portion of the Rendering Item path that should be
	/// ignored when locating views. Default is "/sitecore/layout/renderings".
	/// </remarks>
	public abstract class ConventionController : Controller
	{
		/// <summary>
		/// Fallback action based on known Sitecore and MVC defaults.
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{
			var viewPath = GetViewPath();
			var model = GetModel(RenderingContext.Current.Rendering.Item, RenderingContext.Current.ContextItem);
			return Render(viewPath, model);
		}

		/// <summary>
		/// Assembles the viewPath and the model into an ActionResult.
		/// </summary>
		/// <param name="viewPath">The virtual path to the view on disk.</param>
		/// <param name="model">The model to supply to the View.</param>
		/// <returns></returns>
		protected virtual ActionResult Render(string viewPath, object model)
		{
			return View(viewPath, model);
		}

		protected virtual string GetViewPath()
		{
			var resolver = new ViewResolver(RenderingContext.Current.Rendering.RenderingItem);

			return resolver.ResolveViewPath();
		}

		protected abstract object GetModel(Item datasource, Item contextItem);
	}
}
