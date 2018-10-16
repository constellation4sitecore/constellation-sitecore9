using Sitecore.DependencyInjection;

namespace Constellation.Foundation.ModelMapping
{
	public static class MappingContext
	{
		public static IModelMapper Current
		{
			get
			{
				return (IModelMapper)ServiceLocator.ServiceProvider.GetService(typeof(IModelMapper));
			}
		}
	}
}
