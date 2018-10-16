using Constellation.Feature.PageAnalyticsScripts.Models;
using Constellation.Foundation.ModelMapping;

namespace Constellation.Feature.PageAnalyticsScripts.Controllers
{
	public class PageHeaderScriptsController : PageAnalyticsScriptsController
	{
		public PageHeaderScriptsController(IModelMapper modelMapper) : base(modelMapper)
		{
		}


		protected override string GetScriptToRender(PageAnalyticsScriptsModel model)
		{
			return model.PageHeaderScript;
		}
	}
}
