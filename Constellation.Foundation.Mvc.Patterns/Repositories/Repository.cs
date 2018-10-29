namespace Constellation.Foundation.Mvc.Patterns.Repositories
{
	/// <inheritdoc />
	public abstract class Repository<TModel> : IRepository
	where TModel : class
	{
		/// <summary>
		/// Given the RepositoryContext, retrieve the appropriate data from Sitecore and create an
		/// instance of TModel.
		/// </summary>
		/// <param name="context">The Context of the data request.</param>
		/// <returns>An instance of TModel that represents the results of the request.</returns>
		public abstract TModel GetModel(RepositoryContext context);
	}
}
