using Constellation.Feature.Navigation.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Constellation.Feature.Navigation
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
			serviceCollection.AddTransient<IBreadcrumbNavigationRepository, BreadcrumbNavigationRepository>();
			serviceCollection.AddTransient<IDeclaredNavigationRepository, DeclaredNavigationRepository>();
			serviceCollection.AddTransient<IBranchNavigationRepository, BranchNavigationRepository>();
		}
	}
}
