using Constellation.Feature.Navigation.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Constellation.Feature.Navigation
{
	public class ServicesConfigurator : IServicesConfigurator
	{
		public void Configure(IServiceCollection serviceCollection)
		{
			serviceCollection.AddTransient<IBreadcrumbNavigationRepository, BreadcrumbNavigationRepository>();
			serviceCollection.AddTransient<IDeclaredNavigationRepository, DeclaredNavigationRepository>();
			serviceCollection.AddTransient<IBranchNavigationRepository, BranchNavigationRepository>();
		}
	}
}
