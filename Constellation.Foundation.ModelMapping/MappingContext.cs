using Sitecore.DependencyInjection;

namespace Constellation.Foundation.ModelMapping
{
	/// <summary>
	/// Retrieves the ModelMapper from the Service Provider.
	/// </summary>
	public static class MappingContext
	{
		/// <summary>
		/// The ModelMapper to use for the given scope.
		/// </summary>
		public static IModelMapper Current
		{
			get { return (IModelMapper)ServiceLocator.ServiceProvider.GetService(typeof(IModelMapper)); }
		}
	}
}
