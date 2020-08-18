using Sitecore.Diagnostics;
using System;

namespace Constellation.Foundation.Mvc
{
	/// <summary>
	/// The configuration file settings for OverridingViewPathResolver
	/// </summary>
	public class ViewPathResolverConfiguration
	{
		#region Locals

		private static volatile ViewPathResolverConfiguration _current;

		private static readonly object LockObject = new object();

		#endregion

		#region Constructor
		/// <summary>
		/// Use the "Current" property to get an instance of this class.
		/// </summary>
		protected ViewPathResolverConfiguration()
		{
			//TODO: initialize any lists if they are added later.
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the current settings for OverridingViewPathResolver.
		/// </summary>
		public static ViewPathResolverConfiguration Current
		{
			get
			{
				if (_current == null)
				{
					lock (LockObject)
					{
						if (_current == null)
						{
							_current = CreateNewConfiguration();
						}
					}
				}

				return _current;
			}

		}

		/// <summary>
		/// Gets the path from the root of the Sitecore tree to the Rendering folder where Foundation Renderings live.
		/// </summary>
		public string FoundationRenderingItemPathRoot
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the path from the root of the Sitecore tree to the Rendering folder where Feature Renderings live.
		/// </summary>
		public string FeatureRenderingItemPathRoot
		{
			get;
			private set;
		}

		/// <summary>
		/// gets the path from the root of the Sitecore tree to the Rendering folder where Project Renderings live.
		/// </summary>
		public string ProjectRenderingItemPathRoot
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the path from the root of the web application to the folder where Foundation view files live.
		/// </summary>
		public string FoundationViewPath
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the path from the root of the web application to the folder where Foundation view files live.
		/// </summary>
		public string FeatureViewPath
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the path from the root of the web application to the folder where Project view files live.
		/// Use "{projectName}" in the path to specify where the name of the Project module appears in the path.
		/// </summary>
		public string ProjectViewPath
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a value that determines whether Foundation or Feature views can be dynamically and automatically overridden
		/// by views specific to the Context Site.
		/// </summary>
		public bool AllowSiteOverrides
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the path from the root of the web application to the folder where Site view files live.
		/// </summary>
		public string SiteOverrideViewPath
		{
			get;
			private set;
		}
		#endregion

		#region Methods

		private static ViewPathResolverConfiguration CreateNewConfiguration()
		{
			var output = new ViewPathResolverConfiguration();

			var configNode = Sitecore.Configuration.Factory.GetConfigNode("constellation/viewPathResolver");

			if (configNode == null)
			{
				Log.Warn("Constellation.Foundation.Mvc - No config found for OverridingViewPathResolver.", output);
				return output;
			}

			output.FoundationRenderingItemPathRoot = FormatPath(configNode.Attributes?["foundationRenderingItemPathRoot"]?.Value);
			output.FeatureRenderingItemPathRoot = FormatPath(configNode.Attributes?["featureRenderingItemPathRoot"]?.Value);
			output.ProjectRenderingItemPathRoot = FormatPath(configNode.Attributes?["projectRenderingItemPathRoot"]?.Value);
			output.FoundationViewPath = FormatPath(configNode.Attributes?["foundationViewPath"]?.Value);
			output.FeatureViewPath = FormatPath(configNode.Attributes?["featureViewPath"]?.Value);
			output.ProjectViewPath = FormatPath(configNode.Attributes?["projectViewPath"]?.Value);

			if (bool.TryParse(configNode.Attributes?["allowSiteOverrides"]?.Value, out bool allowOverrides))
			{
				output.AllowSiteOverrides = allowOverrides;
			}

			output.SiteOverrideViewPath = FormatPath(configNode.Attributes?["siteOverrideViewPath"]?.Value);

			return output;
		}


		private static string FormatPath(string path)
		{
			if (!path.EndsWith("/", StringComparison.Ordinal))
			{
				path += "/";
			}

			return path.ToLower();
		}
		#endregion

	}
}
