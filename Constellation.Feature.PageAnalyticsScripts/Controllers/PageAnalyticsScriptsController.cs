using System.Web.Mvc;
using Constellation.Feature.PageAnalyticsScripts.Models;
using Constellation.Foundation.ModelMapping;
using Constellation.Foundation.Mvc;
using Sitecore.Mvc.Presentation;

namespace Constellation.Feature.PageAnalyticsScripts.Controllers
{
	public abstract class PageAnalyticsScriptsController : ConventionController
	{
		public override ActionResult Index()
		{
			var model = (PageAnalyticsScriptsModel)GetModel(RenderingContext.Current.ContextItem, RenderingContext.Current.PageContext.Item);

			return Content(GetScriptToRender(model));
		}

		protected override object GetModel(Sitecore.Data.Items.Item datasource, Sitecore.Data.Items.Item contextItem)
		{
			return datasource.MapToNew<PageAnalyticsScriptsModel>();
		}

		/// <summary>
		/// Use this method to specify which field on the Model to render.
		/// </summary>
		/// <param name="model">The model containing relevant scripts from the Datasource</param>
		/// <returns>The string to render. It could be empty.</returns>
		protected abstract string GetScriptToRender(PageAnalyticsScriptsModel model);
	}
}
