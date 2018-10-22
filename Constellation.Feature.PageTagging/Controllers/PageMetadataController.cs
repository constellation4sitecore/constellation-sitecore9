using System.Text;
using System.Web.Mvc;
using Constellation.Feature.PageTagging.Repositories;
using Sitecore.Mvc.Presentation;

namespace Constellation.Feature.PageTagging.Controllers
{
	/// <summary>
	/// Assign to a Controller Rendering in Sitecore. Will create the following Metatags:
	/// keywords, description, publisher, and author, assuming the Context Item has the following fields:
	/// Keywords, Meta Description, Meta Author, Meta Publisher, or inherits from the Page Metadata template
	/// included in this Package.
	/// </summary>
	public class PageMetadataController : Controller
	{
		#region
		/// <summary>
		/// Creates a new instance of PageMetadataController
		/// </summary>
		/// <param name="repository">An implementation of IMetadataRepository, usually resolved via Dependency Injection.</param>
		public PageMetadataController(IMetadataRepository repository)
		{
			Repository = repository;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the Metadata Repository
		/// </summary>
		protected IMetadataRepository Repository { get; }
		#endregion

		/// <summary>
		/// The Action that should be assigned to the Sitecore Controller Rendering. Note that "Index" is the default
		/// value for Controller Actions in Sitecore configuration.
		/// </summary>
		/// <returns>A string containing any meta tags that had valid values.</returns>
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
