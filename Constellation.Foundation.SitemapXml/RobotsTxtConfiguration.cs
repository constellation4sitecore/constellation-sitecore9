using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Xml;

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
		/// <summary>
		/// Access the current configuration values for robots.txt generation.
		/// </summary>
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

		/// <summary>
		/// List of disallow rows that will be included in all generated robots.txt files.
		/// </summary>
		public ICollection<RobotsTxtRule> GlobalRules { get; private set; }

		/// <summary>
		/// List of disallow rows that will be included in all robots.txt files where the Site
		/// does not have specific overrides.
		/// </summary>
		public ICollection<RobotsTxtRule> DefaultRules { get; private set; }

		/// <summary>
		/// List of allow/disallow rules that will be included in a specific site's robots.txt file.
		/// Note that if a Site defines specific rules, the Default rules will not be included, but the
		/// Global rules will still appear.
		/// </summary>
		public IDictionary<string, ICollection<RobotsTxtRule>> SiteRules { get; private set; }

		#endregion

		private RobotsTxtConfiguration()
		{
			GlobalRules = new List<RobotsTxtRule>();
			DefaultRules = new List<RobotsTxtRule>();
			SiteRules = new Dictionary<string, ICollection<RobotsTxtRule>>();
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

			var globalsNode = Sitecore.Configuration.Factory.GetConfigNode("constellation/robotsTxt/globalRules");

			if (globalsNode != null && globalsNode.HasChildNodes)
			{
				foreach (XmlNode node in globalsNode.ChildNodes)
				{
					output.GlobalRules.Add(new RobotsTxtRule(node));
				}
			}

			var defaultsNode = Sitecore.Configuration.Factory.GetConfigNode("constellation/robotsTxt/defaultRules");

			if (defaultsNode != null && defaultsNode.HasChildNodes)
			{
				foreach (XmlNode node in defaultsNode.ChildNodes)
				{
					output.DefaultRules.Add(new RobotsTxtRule(node));
				}
			}

			var sitesNode = Sitecore.Configuration.Factory.GetConfigNode("constellation/robotsTxt/siteRules");

			if (sitesNode != null && sitesNode.HasChildNodes)
			{
				foreach (XmlNode site in sitesNode.ChildNodes)
				{
					if (!site.HasChildNodes)
					{
						continue;
					}

					var list = new List<RobotsTxtRule>();

					foreach (XmlNode node in site.ChildNodes)
					{
						list.Add(new RobotsTxtRule(node));
					}

					output.SiteRules.Add(site.Name, list);
				}
			}

			return output;
		}

	}
}
