using Sitecore.Data.Items;
using System;

namespace Constellation.Foundation.Mvc
{
	/// <summary>
	/// Creates a virtual path to a View using the RenderingItem provided.
	/// </summary>
	public class ViewPathResolver : IViewPathResolver
	{
		private string _renderingItemPathRoot;

		private string _viewRootPath;

		/// <summary>
		/// Creates a new instance of ViewResolver
		/// </summary>
		/// <param name="renderingItem">The renderingItem to use when creating the view's filepath.</param>
		[Obsolete("Use the empty constructor and supply the RenderingItem to the ResolveViewPath() method.")]
		public ViewPathResolver(RenderingItem renderingItem)
		{
			RenderingItem = renderingItem;
		}

		/// <summary>
		/// Creates a new instance of ViewResolver
		/// </summary>
		public ViewPathResolver()
		{

		}

		#region Properties

		/// <summary>
		/// Gets or sets the Rendering Definition Item to use for View file path resolution.
		/// </summary>
		protected RenderingItem RenderingItem { get; set; }

		/// <inheritdoc />
		/// <summary>
		/// Gets or sets the portion of the RenderingItem's FullPath that should be truncated
		/// when attempting to resolve the Rendering's View path on disk. Default value
		/// is "/sitecore/layout/renderings" Value can be set globally by adding a Sitecore setting for:
		/// "Constellation.Foundation.Mvc.RenderingItemPathRoot"
		/// </summary>
		protected string RenderingItemPathRoot
		{
			get
			{
				if (string.IsNullOrEmpty(_renderingItemPathRoot))
				{
					var setting = Sitecore.Configuration.Settings.GetSetting("Constellation.Foundation.Mvc.RenderingItemPathRoot");

					if (string.IsNullOrEmpty(setting))
					{
						_renderingItemPathRoot = "/sitecore/layout/renderings";
					}
					else
					{
						_renderingItemPathRoot = setting;
					}
				}

				return _renderingItemPathRoot;
			}

			set
			{
				_renderingItemPathRoot = value;
			}
		}


		/// <inheritdoc />
		/// <summary>
		/// Gets or sets the root folder for the application path to the View inferred from the RenderingItem's location in the content tree.
		/// Default Value is "~/Views". Value can be set globally by adding a Sitecore setting for:
		/// "Constellation.Foundation.Mvc.ViewRootPath"
		/// </summary>
		protected string ViewRootPath
		{
			get
			{
				if (string.IsNullOrEmpty(_viewRootPath))
				{

					var setting = Sitecore.Configuration.Settings.GetSetting("Constellation.Foundation.Mvc.ViewRootPath");

					if (string.IsNullOrEmpty(setting))
					{
						_viewRootPath = "~/Views";
					}
					else
					{
						_viewRootPath = setting;
					}
				}

				return _viewRootPath;
			}

			set
			{
				_viewRootPath = value;
			}
		}

		#endregion
		/// <summary>
		/// Creates a Virtual Path to the view associated with the RenderingItem associated with this instance.
		/// Uses RenderingItemPathRoot and ViewRootPath to establish the valid part of the path from Sitecore as well
		/// as the starting point for the virtual path.
		/// </summary>
		/// <returns>The virtual path to the view.</returns>
		[Obsolete("Use ResolveViewPath(RenderingItem) instead.")]
		public string ResolveViewPath()
		{
			if (this.RenderingItem == null)
			{
				throw new Exception("ViewResolver.RenderingItem property was null");
			}

			return ResolveViewPath(this.RenderingItem);
		}

		/// <inheritdoc />
		public string ResolveViewPath(RenderingItem renderingItem)
		{
			var area = renderingItem.InnerItem["Area"];
			var viewRoot = ViewRootPath.ToLower();
			var renderingRoot = RenderingItemPathRoot.ToLower();

			if (!string.IsNullOrEmpty(area))
			{
				renderingRoot = renderingRoot.Replace("$area", area).ToLower();
				viewRoot = viewRoot.Replace("$area", area).ToLower();
			}

			// We need to figure out if there's a Helix folder in the Rendering path and ensure it gets removed from the View's path.
			// Helix folders don't show up in View Paths because MVC Areas aren't complex enough to support it.
			var fullPath = renderingItem.InnerItem.Paths.FullPath;

			if (fullPath.Contains("/Foundation/"))
			{
				renderingRoot = renderingRoot.Replace("$helix", "foundation");
			}

			if (fullPath.Contains("/Feature/"))
			{
				renderingRoot = renderingRoot.Replace("$helix", "feature");
			}

			if (fullPath.Contains("/Project/"))
			{
				renderingRoot = renderingRoot.Replace("$helix", "project");
			}

			var path = NameConverter.ConvertItemPathToClassPath(fullPath).ToLower();

			var truncatedPath = path.Replace(renderingRoot, string.Empty);

			var viewLocation = viewRoot + truncatedPath + ".cshtml";

			return viewLocation;
		}
	}
}
