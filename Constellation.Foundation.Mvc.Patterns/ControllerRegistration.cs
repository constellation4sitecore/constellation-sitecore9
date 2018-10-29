using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Constellation.Foundation.Mvc.Patterns
{
	/// <summary>
	/// Informs the Dependency Injection framework of the services available in this library.
	/// </summary>
	public class ControllerRegistration : IServicesConfigurator
	{
		/// <summary>
		/// Called by the Sitecore Dependency Injection Framework to register services.
		/// </summary>
		/// <param name="serviceCollection">The service collection to append.</param>
		public void Configure(IServiceCollection serviceCollection)
		{
			AddMvcControllers(serviceCollection, Configuration.Current.AssembliesToRegister.ToArray());
		}

		private static void AddMvcControllers(IServiceCollection serviceCollection, params Assembly[] assemblies)
		{
			var controllers = AssemblyCrawler.GetTypesImplementing<IController>(assemblies)
				.Where(controller => controller.Name.EndsWith("Controller", StringComparison.Ordinal));

			foreach (var controller in controllers)
			{
				serviceCollection.AddTransient(controller);
			}

			// adds Web API controller support
			var apiControllers = AssemblyCrawler.GetTypesImplementing<ApiController>(assemblies)
				.Where(controller => controller.Name.EndsWith("Controller", StringComparison.Ordinal));

			foreach (var apiController in apiControllers)
			{
				serviceCollection.AddTransient(apiController);
			}
		}
	}
}
