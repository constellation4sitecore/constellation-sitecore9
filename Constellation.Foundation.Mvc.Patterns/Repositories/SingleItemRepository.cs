using Constellation.Foundation.Caching;
using Constellation.Foundation.ModelMapping;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Constellation.Foundation.Mvc.Patterns.Repositories
{
	/// <summary>
	/// A Repository that generates a ViewModel that is based on a single Item. This repository should be used when the retrieval of
	/// the Item to use for the Model is not something that can be achieved by using the Context Item or setting the Datasource on the
	/// consuming Rendering. Examples include: requiring XPath or a trip to the ContextSearch index to locate the Item to be used. Note that
	/// the parameters for finding the Item should be included in one of the Items provided by the RepositoryContext.
	/// </summary>
	/// <typeparam name="TModel">The Type of the ViewModel that should be returned.</typeparam>
	public abstract class SingleItemRepository<TModel> : CachingRepository<TModel>
	where TModel : class, new()
	{
		/// <summary>
		/// Creates a new instance of SingleItemRepository
		/// </summary>
		/// <param name="modelMapper">The Mapper to use for mapping the Item to the Model. Usually provided by Dependency Injection.</param>
		/// <param name="cacheManager">The Cache Manager to use for storing the completed Model. Usually provided by Depenedency Injection.</param>
		protected SingleItemRepository(IModelMapper modelMapper, ICacheManager cacheManager) : base(cacheManager)
		{
			ModelMapper = modelMapper;
		}

		#region Properties
		/// <summary>
		/// The object to use to map Item Field values to Model properties.
		/// </summary>
		protected IModelMapper ModelMapper { get; }
		#endregion


		/// <summary>
		/// Returns the Model for the request. It should be ready for caching.
		/// </summary>
		/// <param name="context">The context for the request.</param>
		/// <returns></returns>
		protected override TModel GetUncachedModel(RepositoryContext context)
		{
			var item = GetItem(context);

			if (item == null)
			{
				Log.Warn(
					$"{this.GetType().Name}: No Item returned for GetItem call in Repository. Datasource was {context.Datasource.ID}, RequestItem was {context.RequestItem.ID}",
					this);
				return null;
			}

			return ModelMapper.MapItemToNew<TModel>(item);
		}

		/// <summary>
		/// Use this method for Sitecore data retrieval. The resulting Item will be mapped to a ViewModel represented by TModel.
		/// DO NOT ACCESS THE CACHE FROM THIS METHOD.
		/// Note that the parameters for locating the Item should be available from the Items available in the RepositoryContext.
		/// </summary>
		/// <param name="context">The Context of the data request.</param>
		/// <returns>An instance of Item ready for Mapping and caching.</returns>
		protected abstract Item GetItem(RepositoryContext context);
	}
}
