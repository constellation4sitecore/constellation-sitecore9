using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Constellation.Foundation.Mvc
{
	/// <summary>
	/// Informs the Dependency Injection framework of the services available in this library.
	/// </summary>
	public class ServicesConfigurator : IServicesConfigurator
	{
		/// <summary>
		/// Called by the Sitecore Dependency Injection Framework to register services.
		/// </summary>
		/// <param name="serviceCollection">The service collection to append.</param>
		public void Configure(IServiceCollection serviceCollection)
		{
			serviceCollection.AddTransient<IViewPathResolver, ViewPathResolver>();
		}
	}
}
