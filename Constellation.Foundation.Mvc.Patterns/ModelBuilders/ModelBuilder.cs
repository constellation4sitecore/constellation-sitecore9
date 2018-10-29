using Constellation.Foundation.ModelMapping;

namespace Constellation.Foundation.Mvc.Patterns.ModelBuilders
{
	public abstract class ModelBuilder : IModelBuilder
	{
		#region Constructor
		protected ModelBuilder(IModelMapper modelMapper)
		{
			ModelMapper = modelMapper;
		}
		#endregion

		#region Properties
		protected IModelMapper ModelMapper { get; }
		#endregion
	}
}
