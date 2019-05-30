using Constellation.Foundation.Data;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Mvc.Presentation;
using Sitecore.Web;

namespace Constellation.Foundation.Mvc.Patterns
{
	/// <summary>
	/// Used for passing key facts about the current state of Sitecore so that the Repository can
	/// request objects from the correct database or index in the correct language, for the correct site.
	/// </summary>
	public class RepositoryContext
	{
		#region Constructors

		/// <summary>
		/// Creates a new instance of RepositoryContext
		/// </summary>
		protected RepositoryContext()
		{

		}
		#endregion

		#region Properties
		/// <summary>
		/// The "Datasource" of a Rendering, or the Sitecore.Context.Item if the Rendering's datasource is not
		/// explicitly set.
		/// </summary>
		public Item Datasource { get; private set; }

		/// <summary>
		/// The equivalent of Sitecore.Context.Item or RenderingContext.Current.PageContext.Item
		/// </summary>
		public Item RequestItem { get; private set; }

		/// <summary>
		/// The equivalent of Sitecore.Context.Database, or the Database of any Items used to instantiate this context.
		/// </summary>
		public Database Database { get; private set; }

		/// <summary>
		/// The equivalent of Sitecore.Context.Language, or the Language of any Items used to instantiate this context.
		/// </summary>
		public Language Language { get; private set; }

		/// <summary>
		/// The equivalent of Sitecore.Context.Site, or the Site under which the Item used to instantiate this context lives.
		/// This could be null if there's no obvious Site to resolve.
		/// </summary>
		public SiteInfo Site { get; private set; }
		#endregion

		#region Creation Methods

		/// <summary>
		/// Creates a new instance of RepositoryContext using the traditional Sitecore.Context.Current object. Suitable for use
		/// in HttpRequest Pipeline handlers and WebForms applications. Not recommended for MVC Renderings.
		/// </summary>
		/// <returns>An instance of RepositoryContext to pass to a Repository</returns>
		public static RepositoryContext FromSitecoreContext()
		{
			var output = new RepositoryContext
			{
				Datasource = Sitecore.Context.Item,
				RequestItem = Sitecore.Context.Item,
				Database = Sitecore.Context.Database,
				Language = Sitecore.Context.Language,
				Site = Sitecore.Context.Site.SiteInfo
			};

			return output;
		}

		/// <summary>
		/// Creates a new instance of RepositoryContext using the Sitecore.Mvc.Presentation.RenderingContext object. Suitable for use
		/// in MVC Controllers and Views.
		/// </summary>
		/// <param name="context">The RenderingContext to evaluate.</param>
		/// <returns>An instance of RepositoryContext to pass to a Repository.</returns>
		public static RepositoryContext FromRenderingContext(RenderingContext context)
		{
			var output = new RepositoryContext
			{
				Datasource = context.Rendering.Item,
				RequestItem = context.PageContext.Item,
				Database = context.ContextItem.Database,
				Language = context.ContextItem.Language,
				Site = Sitecore.Context.Site.SiteInfo
			};

			return output;
		}

		/// <summary>
		/// Creates a new instance of RepositoryContext using the provided Item and (optionally) Site. Properties like Database and Language
		/// will be inferred from the provided Item. If you do not supply a Site, the Context Site will be used if available, otherwise the system
		/// will attempt to determine the Site that the Item "lives in" naturally. If this fails, the Site property of this context will be null.
		/// </summary>
		/// <param name="context">The Item to evaluate.</param>
		/// <param name="site">(Optional) The Site to assign to the RepositoryContext.</param>
		/// <returns>An instance of RepositoryContext to pass to a Repository.</returns>
		public static RepositoryContext FromItem(Item context, SiteInfo site = null)
		{
			var output = new RepositoryContext
			{
				Datasource = context,
				RequestItem = context,
				Database = context.Database,
				Language = context.Language
			};

			if (site != null)
			{
				output.Site = site;
				return output;
			}

			if (Sitecore.Context.Site != null)
			{
				output.Site = Sitecore.Context.Site.SiteInfo;
				return output;
			}

			output.Site = context.GetSite();
			return output;
		}

		#endregion
	}
}
