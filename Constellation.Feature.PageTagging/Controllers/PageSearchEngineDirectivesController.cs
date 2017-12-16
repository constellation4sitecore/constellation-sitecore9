using Constellation.Feature.PageTagging.Models;
using Constellation.Foundation.ModelMapping;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace Constellation.Feature.PageTagging.Controllers
{
	public class PageSearchEngineDirectivesController : Controller
	{
		public ActionResult Index()
		{
			var model = RenderingContext.Current.ContextItem.MapToNew<PageSearchEngineDirectives>();

			var directives = new List<string>();

			if (model.SearchEngineIndexesPage)
			{
				directives.Add("index");
			}
			else
			{
				directives.Add("noindex");
			}

			if (model.SearchEngineFollowsLinks)
			{
				directives.Add("follow");
			}
			else
			{
				directives.Add("nofollow");
			}

			if (!model.SearchEngineIndexesImages)
			{
				directives.Add("noimageindex");
			}

			if (!model.SearchEngineCanCachePage)
			{
				directives.Add("noarchive");
				directives.Add("nocache");
			}

			if (!model.SearchEngineCanSnippetPage)
			{
				directives.Add("nosnippet");
			}

			if (!model.AllowODPSnippet)
			{
				directives.Add("noodp");
			}

			var builder = new StringBuilder();

			foreach (var directive in directives)
			{
				if (builder.Length > 0)
				{
					builder.Append(",");
				}

				builder.Append(directive);
			}

			return Content($"<meta name=\"robots=\" content=\"{builder}\" />");
		}
	}
}
