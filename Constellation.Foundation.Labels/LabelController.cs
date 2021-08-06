using Sitecore.Mvc.Controllers;
using System.Web.Mvc;

namespace Constellation.Foundation.Labels
{
	/// <summary>
	/// Should be placed as an Item controller for any Data Template Standard Values where the Data Template represents a Labels Item.
	/// </summary>
	public class LabelController : SitecoreController
	{
		/// <summary>
		/// The Action that returns the Label content as JSON.
		/// </summary>
		/// <returns></returns>
		public override System.Web.Mvc.ActionResult Index()
		{
			var item = Sitecore.Context.Item;

			var model = LabelRepository.GetLabels(item);

			return Json(model, JsonRequestBehavior.AllowGet);
		}
	}
}