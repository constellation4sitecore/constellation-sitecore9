using System.Text;
using System.Web.Mvc;
using Constellation.Feature.PageTagging.Repositories;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;

namespace Constellation.Feature.PageTagging.Controllers
{
	/// <summary>
	/// Assign to a Controller Rendering in Sitecore. Will create the following Metatags:
	/// twitter:card, twitter:creator, twitter:site, og:url, og:title, og:description, og:image
	/// assuming the Context Item inherits from the Page Social Metadata template
	/// included in this Package.
	/// </summary>
	public class PageSocialMetadataController : Controller
	{
		private const string MetaName = "<meta name=\"{0}\" content=\"{1}\" />";
		private const string MetaProperty = "<meta property=\"{0}\" content=\"{1}\" />";

		#region Constructor

		/// <summary>
		/// Creates a new instance of PageSocialMetadataController
		/// </summary>
		/// <param name="repository">An instance of ISocialMetadataRepostiory, usually provided by dependency injection.</param>
		public PageSocialMetadataController(ISocialMetadataRepository repository)
		{
			Repository = repository;
		}
		#endregion

		#region Properties
		/// <summary>
		/// The ISocialMetadataRepository to use for converting Item fields to the PageSocialMetadata model properties.
		/// </summary>
		protected ISocialMetadataRepository Repository { get; }
		#endregion

		/// <summary>
		/// The Action that should be assigned to the Sitecore Controller Rendering. Note that "Index" is the default
		/// value for Controller Actions in Sitecore configuration.
		/// </summary>
		/// <returns>A string containing the meta tags for which the Item had valid values.</returns>
		public ActionResult Index()
		{
			var model = Repository.GetMetadata(RenderingContext.Current.ContextItem);

			var builder = new StringBuilder();

			if (!string.IsNullOrEmpty(model.TwitterCreator) || !string.IsNullOrEmpty(model.TwitterSite))
			{
				builder.AppendLine(string.Format(MetaName, "twitter:card", model.TwitterCardType));

				if (!string.IsNullOrEmpty(model.TwitterCreator))
				{
					builder.AppendLine(string.Format(MetaName, "twitter:creator", model.TwitterCreator));
				}

				if (!string.IsNullOrEmpty(model.TwitterSite))
				{
					builder.AppendLine(string.Format(MetaName, "twitter:site", model.TwitterSite));
				}
			}

			var options = LinkManager.GetDefaultUrlBuilderOptions();
			options.AlwaysIncludeServerUrl = true;
			options.SiteResolving = true;
			options.LowercaseUrls = true;


			var url = LinkManager.GetItemUrl(RenderingContext.Current.PageContext.Item, options);

			builder.AppendLine(string.Format(MetaProperty, "og:url", url));
			builder.AppendLine(string.Format(MetaProperty, "og:title", model.BrowserTitle));
			if (!string.IsNullOrEmpty(model.MetaDescription))
			{
				builder.AppendLine(string.Format(MetaProperty, "og:description", model.MetaDescription));
			}

			if (!string.IsNullOrEmpty(model.SocialThumbnail))
			{
				builder.AppendLine(string.Format(MetaProperty, "og:image", model.SocialThumbnail));
			}

			return Content(builder.ToString());
		}
	}
}
