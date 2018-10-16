using Constellation.Feature.PageAnalyticsScripts.Models;
using Constellation.Foundation.ModelMapping;

namespace Constellation.Feature.PageAnalyticsScripts.Controllers
{
	public class BodyBottomScriptsController : PageAnalyticsScriptsController
	{
		public BodyBottomScriptsController(IModelMapper modelMapper) : base(modelMapper)
		{
		}

		protected override string GetScriptToRender(PageAnalyticsScriptsModel model)
		{
			return model.BodyBottomScript;
		}
	}
}
