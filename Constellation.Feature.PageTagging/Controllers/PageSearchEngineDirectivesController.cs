using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Constellation.Feature.PageTagging.Models;
using Constellation.Foundation.ModelMapping;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Constellation.Feature.PageTagging.Controllers
{
	public class PageSearchEngineDirectivesController : Controller
	{
		#region
		public PageSearchEngineDirectivesController(IModelMapper modelMapper)
		{
			ModelMapper = modelMapper;
		}
		#endregion

		#region Properties
		protected IModelMapper ModelMapper { get; }
		#endregion

		public ActionResult Index()
		{
			var model = BuildModel(RenderingContext.Current.PageContext.Item);
			var directives = GetDirectives(model);
			var content = GetContent(directives);

			return Content($"<meta name=\"robots=\" content=\"{content}\" />");
		}

		private PageSearchEngineDirectives BuildModel(Item contextItem)
		{
			return ModelMapper.MapItemToNew<PageSearchEngineDirectives>(contextItem);
		}

		private static IEnumerable<string> GetDirectives(PageSearchEngineDirectives model)
		{

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

			return directives;
		}

		private static string GetContent(IEnumerable<string> directives)
		{
			var builder = new StringBuilder();

			foreach (var directive in directives)
			{
				if (builder.Length > 0)
				{
					builder.Append(",");
				}

				builder.Append(directive);
			}

			return builder.ToString();
		}
	}
}
