using System;
using System.Reflection;
using Constellation.Foundation.Mvc.Patterns.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Constellation.Foundation.Mvc.Patterns
{
	/// <summary>
	/// Informs the Dependency Injection framework of the services available in this library.
	/// </summary>
	public class RepositoryRegistration : IServicesConfigurator
	{
		/// <summary>
		/// Called by the Sitecore Dependency Injection Framework to register services.
		/// </summary>
		/// <param name="serviceCollection">The service collection to append.</param>
		public void Configure(IServiceCollection serviceCollection)
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			AddRepositories(serviceCollection, assemblies);
		}

		private void AddRepositories(IServiceCollection serviceCollection, params Assembly[] assemblies)
		{
			var repositories = AssemblyCrawler.GetTypesImplementing<IRepository>(assemblies);

			foreach (var repository in repositories)
			{
				serviceCollection.AddTransient(repository);
			}
		}
	}
}
