using Constellation.Feature.Redirects.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Constellation.Feature.Redirects
{
	/// <summary>
	/// Registers this library's services with Dependency Injection.
	/// </summary>
	public class ServiceConfigurator : IServicesConfigurator
	{
		/// <summary>
		/// Adds the services to the Service Collection
		/// </summary>
		/// <param name="serviceCollection">The Service Collection to append.</param>
		public void Configure(IServiceCollection serviceCollection)
		{
			serviceCollection.AddTransient(typeof(PageRedirectController));
		}
	}
}
