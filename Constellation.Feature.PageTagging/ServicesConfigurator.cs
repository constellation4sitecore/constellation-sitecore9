using Constellation.Feature.PageTagging.Controllers;
using Constellation.Feature.PageTagging.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Constellation.Feature.PageTagging
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
			serviceCollection.AddTransient<IMetadataRepository, MetadataRepository>();
			serviceCollection.AddTransient<ISocialMetadataRepository, SocialMetadataRepository>();
			serviceCollection.AddTransient(typeof(PageMetadataController));
			serviceCollection.AddTransient(typeof(PageSearchEngineDirectivesController));
			serviceCollection.AddTransient(typeof(PageSocialMetadataController));
		}
	}
}
