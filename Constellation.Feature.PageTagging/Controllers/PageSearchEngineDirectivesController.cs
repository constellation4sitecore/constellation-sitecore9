using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Constellation.Feature.PageTagging.Models;
using Constellation.Foundation.ModelMapping;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Constellation.Feature.PageTagging.Controllers
{
	/// <summary>
	/// Assign to a Controller Rendering in Sitecore. Will create the
	/// meta:robots tag based on the checked values on an Item that implements the
	/// Page Search Engine Directives template.
	/// </summary>
	public class PageSearchEngineDirectivesController : Controller
	{
		#region
		/// <summary>
		/// Creates a new instance of PageSearchEngineDirectivesController
		/// </summary>
		/// <param name="modelMapper">An instance of IModelMapper, usually provided by dependency injection.</param>
		public PageSearchEngineDirectivesController(IModelMapper modelMapper)
		{
			ModelMapper = modelMapper;
		}
		#endregion

		#region Properties
		/// <summary>
		/// The instance of IModelMapper to use when mapping Item fields to model properties.
		/// </summary>
		protected IModelMapper ModelMapper { get; }
		#endregion

		/// <summary>
		/// The Action that should be assigned to the Sitecore Controller Rendering. Note that "Index" is the default
		/// value for Controller Actions in Sitecore configuration.
		/// </summary>
		/// <returns>A string containing the meta:robots tags with valid values.</returns>
		public ActionResult Index()
		{
			var model = BuildModel(RenderingContext.Current.PageContext.Item);
			var directives = GetDirectives(model);
			var content = GetContent(directives);

			return Content($"<meta name=\"robots\" content=\"{content}\" />");
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
