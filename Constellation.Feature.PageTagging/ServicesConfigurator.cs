using Constellation.Feature.PageTagging.Controllers;
using Constellation.Feature.PageTagging.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Constellation.Feature.PageTagging
{
	public class ServicesConfigurator : IServicesConfigurator
	{
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
