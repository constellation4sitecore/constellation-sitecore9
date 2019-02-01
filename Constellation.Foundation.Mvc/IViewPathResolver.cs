using Sitecore.Data.Items;

namespace Constellation.Foundation.Mvc
{
	/// <summary>
	/// Creates a virtual path to a View using the RenderingItem provided.
	/// </summary>
	public interface IViewPathResolver
	{
		/// <summary>
		/// Creates a Virtual Path to the view associated with the RenderingItem associated with this instance.
		/// Uses RenderingItemPathRoot and ViewRootPath to establish the valid part of the path from Sitecore as well
		/// as the starting point for the virtual path.
		/// </summary>
		/// <param name="renderingItem">The renderingItem to use when creating the view's filepath.</param>
		/// <returns>The virtual path to the view.</returns>
		string ResolveViewPath(RenderingItem renderingItem);
	}
}
