using Constellation.Foundation.Mvc.Patterns.Repositories;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Constellation.Foundation.Mvc.Patterns.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// A controller where the rendering represents a list of Items.
	/// </summary>
	/// <typeparam name="TListRecord">The ViewModel to use for individual Items in the list.</typeparam>
	public abstract class ItemListController<TListRecord> : ConventionController
		where TListRecord : class, new()
	{
		#region Constructor
		/// <summary>
		/// Creates a new instance of ItemListController
		/// </summary>
		/// <param name="viewPathResolver">The view path resolver.</param>
		/// <param name="repository">The repository used to retrieve the collection of ViewModels to display.</param>
		protected ItemListController(IViewPathResolver viewPathResolver, ItemListRepository<TListRecord> repository) : base(viewPathResolver)
		{
			Repository = repository;
		}
		#endregion

		#region Properties
		/// <summary>
		/// The repository used to query Sitecore and get the result set.
		/// </summary>
		protected ItemListRepository<TListRecord> Repository { get; }
		#endregion


		/// <summary>
		/// Uses the repository to get the Items, cast them to the appropriate view model, cache the results and return the model.
		/// </summary>
		/// <param name="datasource">The Rendering's Datasource Item (resolved) could be the Context Item</param>
		/// <param name="contextItem">The Page Item or Sitecore Context Item.</param>
		/// <returns>A collection of TListRecord.</returns>
		protected override object GetModel(Item datasource, Item contextItem)
		{
			return Repository.GetModel(RepositoryContext.FromRenderingContext(RenderingContext.Current));
		}
	}
}
