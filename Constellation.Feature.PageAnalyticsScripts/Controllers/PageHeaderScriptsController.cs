using Constellation.Feature.PageAnalyticsScripts.Models;

namespace Constellation.Feature.PageAnalyticsScripts.Controllers
{
	public class PageHeaderScriptsController : PageAnalyticsScriptsController
	{
		protected override string GetScriptToRender(PageAnalyticsScriptsModel model)
		{
			return model.PageHeaderScript;
		}
	}
}
