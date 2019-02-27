using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sitecore.Abstractions;
using Sitecore.DependencyInjection;

namespace Constellation.Foundation.Linking
{
	/// <summary>
	/// Sitecore Initialization routine that registers the SwitchingLinkManager with the services provider.
	/// </summary>
	public class ReplaceLinkManagerWithSwitchingLinkManagerConfigurator : IServicesConfigurator
	{
		/// <summary>
		/// Registers the SwitchingLinkManager with requests for BaseLinkManager
		/// </summary>
		/// <param name="serviceCollection">The Services Collection to register with.</param>
		public void Configure(IServiceCollection serviceCollection)
		{
			var service = new ServiceDescriptor(typeof(BaseLinkManager), typeof(SwitchingLinkManager), ServiceLifetime.Singleton);
			serviceCollection.Replace(service);
		}
	}
}
