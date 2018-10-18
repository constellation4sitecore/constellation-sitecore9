using Constellation.Feature.PageAnalyticsScripts.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Constellation.Feature.PageAnalyticsScripts
{
	public class ServicesConfigurator : IServicesConfigurator
	{
		public void Configure(IServiceCollection serviceCollection)
		{
			serviceCollection.AddTransient(typeof(BodyBottomScriptsController));
			serviceCollection.AddTransient(typeof(BodyTopScriptsController));
			serviceCollection.AddTransient(typeof(PageHeaderScriptsController));
		}
	}
}
