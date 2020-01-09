using System.Xml;

namespace Constellation.Foundation.SitemapXml
{
	/// <summary>
	/// Represents a rule in a Robots.txt file.
	/// </summary>
	public class RobotsTxtRule
	{
		/// <summary>
		/// Defines whether the Rule is an allow or deny indexing.
		/// </summary>
		public enum RulePermission
		{
			/// <summary>
			/// Site crawlers should index this path.
			/// </summary>
			Allow,
			/// <summary>
			/// Site crawlers should not index this path.
			/// </summary>
			Deny
		}

		#region Constructors

		/// <summary>
		/// Creates a new empty instance of RobotsTxtRule
		/// </summary>
		public RobotsTxtRule()
		{

		}

		/// <summary>
		/// Creates a new instance of RobotsTxtRule with values from the provided configuration node.
		/// </summary>
		/// <param name="node"></param>
		public RobotsTxtRule(XmlNode node)
		{
			Path = node.InnerText;

			switch (node.Name)
			{
				case "allow":
					Permission = RulePermission.Allow;
					break;
				case "disallow":
					Permission = RulePermission.Deny;
					break;
				default:
					Permission = RulePermission.Allow;
					break;
			}
		}
		#endregion

		/// <summary>
		/// The path for the rule.
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// The permission for the rule.
		/// </summary>
		public RulePermission Permission { get; set; }
	}
}
