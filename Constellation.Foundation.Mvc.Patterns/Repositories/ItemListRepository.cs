using Constellation.Foundation.Caching;
using Constellation.Foundation.Mvc.Patterns.ModelBuilders;

namespace Constellation.Foundation.Mvc.Patterns.Repositories
{
	public abstract class ItemListRepository : Repository
	{
		protected ItemListRepository(RepositoryContext context, ICacheManager cacheManager, IModelBuilder modelBuilder) : base(context, cacheManager, modelBuilder)
		{
		}
	}
}
