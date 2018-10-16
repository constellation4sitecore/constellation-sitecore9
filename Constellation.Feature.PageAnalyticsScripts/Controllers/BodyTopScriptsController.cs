using Constellation.Feature.PageAnalyticsScripts.Models;
using Constellation.Foundation.ModelMapping;

namespace Constellation.Feature.PageAnalyticsScripts.Controllers
{
	public class BodyTopScriptsController : PageAnalyticsScriptsController
	{
		public BodyTopScriptsController(IModelMapper modelMapper) : base(modelMapper)
		{
		}

		protected override string GetScriptToRender(PageAnalyticsScriptsModel model)
		{
			return model.BodyTopScript;
		}
	}
}
