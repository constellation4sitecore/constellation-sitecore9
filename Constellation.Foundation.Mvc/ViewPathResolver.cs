using Constellation.Foundation.Data;
using Sitecore.Data.Items;
using System;
using System.IO;
using System.Web;
// ReSharper disable StringLiteralTypo

namespace Constellation.Foundation.Mvc
{
	/// <summary>
	/// Creates a virtual path to a View using the RenderingItem provided.
	/// </summary>
	public class ViewPathResolver : IViewPathResolver
	{
		private enum ModuleType
		{
			Foundation,
			Feature,
			Project,
			Unknown
		}


		/// <inheritdoc />
		public string ResolveViewPath(RenderingItem renderingItem)
		{
			var config = ViewPathResolverConfiguration.Current;
			string renderingRootPath;
			string viewRoot;
			string moduleName;
			string lowerPath;


			var fullPath = renderingItem.InnerItem.Paths.FullPath.ToLower();
			var moduleType = GetModuleTypeFromRenderingPath(fullPath);

			switch (moduleType)
			{
				case ModuleType.Project:
					renderingRootPath = config.ProjectRenderingItemPathRoot.ToLower();
					viewRoot = config.ProjectViewPath.ToLower();
					break;
				case ModuleType.Feature:
					renderingRootPath = config.FeatureRenderingItemPathRoot.ToLower();
					viewRoot = config.FeatureViewPath.ToLower();
					break;
				case ModuleType.Foundation:
					renderingRootPath = config.FoundationRenderingItemPathRoot.ToLower();
					viewRoot = config.FoundationViewPath.ToLower();
					break;
				default:
					// Not in a Helix folder, so use stock View resolution.
					return renderingItem.Name.AsClassName();
			}

			lowerPath = NameConverter.ConvertItemPathToClassPath(fullPath.Replace(renderingRootPath, ""));
			moduleName = lowerPath.Substring(0, lowerPath.IndexOf("/", StringComparison.Ordinal));
			lowerPath = lowerPath.Replace(moduleName + "/", "");

			viewRoot = viewRoot.Replace("{modulename}", moduleName);
			var siteName = Sitecore.Context.Site.Name.ToLower();

			// This section allows a Site to override a View from a higher-level module (Feature or Foundation usually)
			// Since "sites" are also typically "projects" we need to make sure that we don't self-override.
			if (config.AllowSiteOverrides && moduleName != siteName)
			{
				var viewOverrideRoot = config.SiteOverrideViewPath.ToLower();
				viewOverrideRoot = viewOverrideRoot.Replace("{sitename}", siteName);
				viewOverrideRoot = viewOverrideRoot.Replace("{modulename}", moduleName);

				var overridePath = viewOverrideRoot + lowerPath + ".cshtml";
				var physicalPath = HttpContext.Current.Server.MapPath(overridePath);
				if (File.Exists(physicalPath))
				{
					return overridePath;
				}
			}

			var viewPath = viewRoot + lowerPath + ".cshtml";

			return viewPath;
		}


		private static ModuleType GetModuleTypeFromRenderingPath(string fullPath)
		{
			var path = fullPath.ToLower();

			if (path.Contains("/project/"))
			{
				return ModuleType.Project;
			}

			if (path.Contains("/feature/"))
			{
				return ModuleType.Feature;
			}

			if (path.Contains("/foundation/"))
			{
				return ModuleType.Foundation;
			}

			return ModuleType.Unknown;
		}
	}
}
