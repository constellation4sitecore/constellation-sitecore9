using System;
using System.Collections.Generic;
using System.Xml;
using Sitecore.Diagnostics;

namespace Constellation.Foundation.SitemapXml
{
	/// <summary>
	/// The robots txt handler configuration.
	/// </summary>
	public class RobotsTxtConfiguration
	{
		#region Locals
		private static volatile RobotsTxtConfiguration _current;

		private static object _lockObject = new object();
		#endregion

		#region Properties
		public static RobotsTxtConfiguration Current
		{
			get
			{
				if (_current == null)
				{
					lock (_lockObject)
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

		public bool Allowed { get; private set; }

		public ICollection<string> GlobalDisallows { get; private set; }

		public IDictionary<string, ICollection<string>> SiteDisallows { get; private set; }

		#endregion

		private RobotsTxtConfiguration()
		{
			GlobalDisallows = new List<string>();
			SiteDisallows = new Dictionary<string, ICollection<string>>();
		}

		private static RobotsTxtConfiguration CreateNewConfiguration()
		{
			RobotsTxtConfiguration output = new RobotsTxtConfiguration();

			var root = Sitecore.Configuration.Factory.GetConfigNode("constellation/robotsTxt");

			if (root == null)
			{
				var ex = new Exception(" No constellation/robotsTxt configuration found.");
				Log.Error("Constellation.Foundation.SitemapXml: error loading configuration.", ex, output);
				throw ex;
			}

			if (bool.TryParse(root.Attributes?["allowed"]?.Value, out var allowed))
			{
				output.Allowed = allowed;
			}

			var globalsNode = Sitecore.Configuration.Factory.GetConfigNode("constellation/robotsTxt/globalDisallows");

			if (globalsNode != null && globalsNode.HasChildNodes)
			{
				foreach (XmlNode disallow in globalsNode.ChildNodes)
				{
					output.GlobalDisallows.Add(disallow.InnerText);
				}
			}

			var sitesNode = Sitecore.Configuration.Factory.GetConfigNode("constellation/robotsTxt/siteDisallows");

			if (sitesNode != null && sitesNode.HasChildNodes)
			{
				foreach (XmlNode site in sitesNode.ChildNodes)
				{
					if (!site.HasChildNodes)
					{
						continue;
					}

					var list = new List<string>();

					foreach (XmlNode disallow in site.ChildNodes)
					{
						list.Add(disallow.InnerText);
					}

					output.SiteDisallows.Add(site.Name, list);
				}
			}

			return output;
		}

	}
}
