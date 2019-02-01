using System;
using Sitecore.Data.Items;

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
			var viewRoot = ViewRootPath;
			var renderingRoot = RenderingItemPathRoot.ToLower();

			if (!string.IsNullOrEmpty(area))
			{
				renderingRoot = RenderingItemPathRoot.Replace("$Area", area).ToLower();
				viewRoot = viewRoot.Replace("$Area", area).ToLower();
			}

			var path = NameConverter.ConvertItemPathToClassPath(renderingItem.InnerItem.Paths.FullPath).ToLower();
			var truncatedPath = path.Replace(renderingRoot, string.Empty);

			var viewLocation = viewRoot + truncatedPath + ".cshtml";

			return viewLocation;
		}
	}
}
