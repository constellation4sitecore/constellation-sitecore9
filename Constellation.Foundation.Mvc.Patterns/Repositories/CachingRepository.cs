using System.Text;
using Constellation.Foundation.Caching;
using Constellation.Foundation.Mvc.Patterns.ModelBuilders;

namespace Constellation.Foundation.Mvc.Patterns.Repositories
{
	public abstract class CachingRepository<TModel> : Repository<TModel>
	where TModel : class, new()
	{
		#region Constructors
		protected CachingRepository(ICacheManager cacheManager, IModelBuilder modelBuilder) : base(modelBuilder)
		{
			CacheManager = cacheManager;
		}
		#endregion

		#region Properties
		protected ICacheManager CacheManager { get; }

		protected IModelBuilder ModelBuilder { get; }
		#endregion

		public override TModel GetModel(RepositoryContext context)
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
				CacheManager.Add(key, model, null);
			}


		}

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
