using Sitecore.Web;

namespace Constellation.Feature.PageTagging
{
	/// <summary>
	/// Extension Methods for Sitecore's SiteInfo object.
	/// </summary>
	public static class SiteInfoExtensions
	{

		/// <summary>
		/// Returns the value of the "robotsMetatagOverride" attribute on a Sitecore configuration "site" element, if present.
		/// </summary>
		/// <param name="site">The SiteInfo to inspect for the attribute.</param>
		/// <returns>The value of the attribute or null.</returns>
		public static string GetRobotsMetatagOverride(this SiteInfo site)
		{
			return site.Properties["robotsMetatagOverride"];
		}
	}
}
