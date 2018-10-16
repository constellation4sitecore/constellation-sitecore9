using System.Text;
using System.Web.Mvc;
using Constellation.Feature.PageTagging.Repositories;
using Sitecore.Mvc.Presentation;

namespace Constellation.Feature.PageTagging.Controllers
{
	public class PageMetadataController : Controller
	{
		#region
		public PageMetadataController(IMetadataRepository repository)
		{
			Repository = repository;
		}
		#endregion

		#region Properties
		protected IMetadataRepository Repository { get; }
		#endregion

		public ActionResult Index()
		{
			var model = Repository.GetMetadata(RenderingContext.Current.ContextItem);

			var builder = new StringBuilder();

			if (!string.IsNullOrEmpty(model.Keywords))
			{
				builder.AppendLine($"<meta name=\"keywords\" content=\"{model.Keywords}\" />");
			}
			if (!string.IsNullOrEmpty(model.MetaDescription))
			{
				builder.AppendLine($"<meta name=\"description\" content=\"{model.MetaDescription}\" />");
			}
			if (model.HasValidPublisher)
			{
				builder.AppendLine($"<meta name=\"publisher\" content=\"{model.MetaPublisher}\" />");
			}
			if (model.HasValidAuthor)
			{
				builder.AppendLine($"<meta name=\"author\" content=\"{model.MetaAuthor}\" />");
			}

			return Content(builder.ToString());
		}
	}
}
