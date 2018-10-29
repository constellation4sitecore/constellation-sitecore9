using Constellation.Foundation.Caching;
using Constellation.Foundation.Mvc.Patterns.ModelBuilders;

namespace Constellation.Foundation.Mvc.Patterns.Repositories
{
	public class SingleItemRepository : Repository
	{
		public SingleItemRepository(RepositoryContext context, ICacheManager cacheManager, IModelBuilder modelBuilder) : base(context, cacheManager, modelBuilder)
		{
		}
	}
}
