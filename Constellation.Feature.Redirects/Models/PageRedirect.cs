using Constellation.Foundation.ModelMapping.MappingAttributes;
using Sitecore.Data;

namespace Constellation.Feature.Redirects.Models
{
	/// <summary>
	/// An Item that represents an Http Redirect to a location other than its own URL.
	/// </summary>
	public class PageRedirect
	{
		/// <summary>
		/// The Name of the Item.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The DisplayName of the Item.
		/// </summary>
		public string DisplayName { get; set; }

		/// <summary>
		/// The ID of the Item.
		/// </summary>
		public ID ID { get; set; }

		/// <summary>
		/// The Url that Sitecore should redirect the request to when this Item's URL is requested.
		/// </summary>
		public string RedirectLinkUrl { get; set; }

		/// <summary>
		/// Indicates whether the redirect should include the "301 Permanent" response.
		/// </summary>
		public bool IsPermanent { get; set; }

		/// <summary>
		/// The value to use when rendering this Item in navigation.
		/// </summary>
		[RawValueOnly]
		public string NavigationTitle { get; set; }
	}
}
