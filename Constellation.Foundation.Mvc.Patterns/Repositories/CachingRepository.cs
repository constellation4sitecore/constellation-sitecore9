using System;
using System.Text;
using Constellation.Foundation.Caching;

namespace Constellation.Foundation.Mvc.Patterns.Repositories
{
	/// <inheritdoc/>
	/// <summary>
	/// This Repository type will cache the results for use in future requests.
	/// </summary>
	/// <typeparam name="TModel">The ViewModel to return.</typeparam>
	public abstract class CachingRepository<TModel> : Repository<TModel>
	where TModel : class
	{
		#region Constructors
		/// <summary>
		/// Creates a new instance of CachingRepository.
		/// </summary>
		/// <param name="cacheManager">The CacheManager to use for caching results.</param>
		protected CachingRepository(ICacheManager cacheManager)
		{
			CacheManager = cacheManager;
		}
		#endregion

		#region Properties
		/// <summary>
		/// The object to use for accessing the cache.
		/// </summary>
		protected ICacheManager CacheManager { get; }
		#endregion

		/// <summary>
		/// Given the RepositoryContext, retrieve the appropriate data from Sitecore and create an
		/// instance of TModel. This call will cache the results of the request for 4 hours or until the next
		/// Sitecore publish job.
		/// </summary>
		/// <param name="context">The Context of the data request.</param>
		/// <returns>An instance of TModel that represents the results of the request.</returns>
		public override TModel GetModel(RepositoryContext context)
		{
			return GetModel(context, DateTime.Now.AddHours(4));
		}

		/// <summary>
		/// Given the RepositoryContext, retrieve the appropriate data from Sitecore and create an
		/// instance of TModel. This call will cache the results of the request until the provided expiration DateTime or
		/// the next Sitecore publish job.
		/// </summary>
		/// <param name="context">The Context of the data request.</param>
		/// <param name="cacheExpires">The DateTime when the cached results should expire.</param>
		/// <returns>An instance of TModel that represents the results of the request.</returns>
		public TModel GetModel(RepositoryContext context, DateTime cacheExpires)
		{
			var key = GetKey(context);

			var model = CacheManager.Get<TModel>(key);

			if (model != null)
			{
				return model;
			}

			model = GetUncachedModel(context);

			if (model != null)
			{
				CacheManager.Add(key, model, DateTime.Now.AddHours(4));
			}

			return model;
		}

		/// <summary>
		/// Use this method for Sitecore data retrieval and Model generation. DO NOT ACCESS THE CACHE FROM THIS METHOD.
		/// </summary>
		/// <param name="context">The Context of the data request.</param>
		/// <returns>An instance of TModel that will be cached upstream.</returns>
		protected abstract TModel GetUncachedModel(RepositoryContext context);


		private string GetKey(RepositoryContext context)
		{
			var builder = new StringBuilder(this.GetType().Name);

			if (context.Site != null)
			{
				builder.Append(context.Site.Name);
			}

			builder.Append(context.Database.Name);
			builder.Append(context.Language.Name);
			builder.Append(context.Datasource.ID);
			builder.Append(context.RequestItem.ID);

			return builder.ToString();
		}
	}
}
