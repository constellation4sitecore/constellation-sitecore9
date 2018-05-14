using Constellation.Feature.PageAnalyticsScripts.Models;

namespace Constellation.Feature.PageAnalyticsScripts.Controllers
{
	public class BodyBottomScriptsController : PageAnalyticsScriptsController
	{
		protected override string GetScriptToRender(PageAnalyticsScriptsModel model)
		{
			return model.BodyBottomScript;
		}
	}
}
