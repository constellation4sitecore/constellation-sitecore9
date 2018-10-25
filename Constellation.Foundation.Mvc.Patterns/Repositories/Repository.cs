using Constellation.Foundation.Mvc.Patterns.ModelBuilders;

namespace Constellation.Foundation.Mvc.Patterns.Repositories
{
	public abstract class Repository : IRepository
	{
		#region Constructors
		protected Repository()
		{

		}
		#endregion

		#region Properties
		protected IModelBuilder ModelBuilder { get; }
		#endregion


	}
}
