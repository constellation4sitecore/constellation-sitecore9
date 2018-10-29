using Constellation.Foundation.Mvc.Patterns.ModelBuilders;

namespace Constellation.Foundation.Mvc.Patterns.Repositories
{
	public abstract class Repository<TModel> : IRepository<TModel>
	where TModel : class, new()
	{
		#region Constructors
		protected Repository(IModelBuilder modelBuilder)
		{
			ModelBuilder = modelBuilder;
		}
		#endregion

		#region Properties
		protected IModelBuilder ModelBuilder { get; }
		#endregion

		public abstract TModel GetModel(RepositoryContext context);
	}
}
