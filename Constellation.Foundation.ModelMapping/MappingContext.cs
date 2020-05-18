//using Sitecore.DependencyInjection;

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
			get
			{
				return new ModelMapper();

				// Dependency Injection is disabled because this library was originally set up for Sitecore 9.x Dependency Injection.
				//return (IModelMapper)ServiceLocator.ServiceProvider.GetService(typeof(IModelMapper));
			}
		}
	}
}
