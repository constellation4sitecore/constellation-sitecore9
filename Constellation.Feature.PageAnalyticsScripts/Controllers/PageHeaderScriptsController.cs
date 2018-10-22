using Constellation.Feature.PageAnalyticsScripts.Models;
using Constellation.Foundation.ModelMapping;

namespace Constellation.Feature.PageAnalyticsScripts.Controllers
{
	/// <summary>
	/// Assign to a Controller Rendering in Sitecore.
	/// Assumes the Datasource Item inherits from the Page Analytics Scripts template
	/// included in this Package.
	/// Renders the value of the Page Header Scripts field to the page.
	/// </summary>
	public class PageHeaderScriptsController : PageAnalyticsScriptsController
	{
		/// <summary>
		/// Creates a new instance of PageHeaderScriptsController
		/// </summary>
		/// <param name="modelMapper">The IModelMapper instance to use for Item to Model mapping, usually supplied through Dependency Injection.</param>
		public PageHeaderScriptsController(IModelMapper modelMapper) : base(modelMapper)
		{
		}

		/// <summary>
		/// Returns the Page Header Script.
		/// </summary>
		/// <param name="model">The model to inspect.</param>
		/// <returns>The scripts in the PageHeaderScript property.</returns>
		protected override string GetScriptToRender(PageAnalyticsScriptsModel model)
		{
			return model.PageHeaderScript;
		}
	}
}
