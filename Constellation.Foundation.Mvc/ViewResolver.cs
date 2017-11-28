using Sitecore.Data.Items;

namespace Constellation.Foundation.Mvc
{
	/// <summary>
	/// Creates a virtual path to a View using the RenderingItem provided.
	/// </summary>
	public class ViewResolver
	{
		private string _renderingItemPathRoot;

		private string _viewRootPath;

		/// <summary>
		/// Creates a new instance of ViewResolver
		/// </summary>
		/// <param name="renderingItem">The renderingItem to use when creating the view's filepath.</param>
		public ViewResolver(RenderingItem renderingItem)
		{
			RenderingItem = renderingItem;
		}

		#region Properties

		protected RenderingItem RenderingItem { get; set; }

		/// <summary>
		/// Gets or sets the portion of the RenderingItem's FullPath that should be truncated
		/// when attempting to resolve the Rendering's View path on disk. Default value
		/// is "/sitecore/layout/renderings" Value can be set globally by adding a Sitecore setting for:
		/// "Constellation.Foundation.Mvc.RenderingItemPathRoot"
		/// </summary>
		public string RenderingItemPathRoot
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


		/// <summary>
		/// Gets or sets the root folder for the application path to the View inferred from the RenderingItem's location in the content tree.
		/// Default Value is "~/Views". Value can be set globally by adding a Sitecore setting for:
		/// "Constellation.Foundation.Mvc.ViewRootPath"
		/// </summary>
		public string ViewRootPath
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
		public string ResolveViewPath()
		{

			var area = RenderingItem.InnerItem["Area"];
			var viewRoot = ViewRootPath;
			var renderingRoot = RenderingItemPathRoot;

			if (!string.IsNullOrEmpty(area))
			{
				renderingRoot = renderingRoot.Replace("$Area", area).ToLower();
				viewRoot = viewRoot.Replace("$Area", area).ToLower();
			}

			var path = NameConverter.ConvertItemPathToClassPath(RenderingItem.InnerItem.Paths.FullPath).ToLower();
			var truncatedPath = path.Replace(renderingRoot, string.Empty);

			var viewLocation = viewRoot + truncatedPath + ".cshtml";

			return viewLocation;
		}
	}
}
