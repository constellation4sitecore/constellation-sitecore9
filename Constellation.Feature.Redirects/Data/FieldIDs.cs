using Sitecore.Data;

namespace Constellation.Feature.Redirects.Data
{
	/// <summary>
	/// IDs of system-critical Fields.
	/// </summary>
	public static class FieldIDs
	{
		/// <summary>
		/// The ID of the Site Name field
		/// </summary>
		public static ID SiteName = new ID("{CD7E312B-B8A7-4A77-872D-535D3DD8E34E}");

		/// <summary>
		/// The ID of the Old Url field
		/// </summary>
		public static ID OldUrl = new ID("{1457500E-AEE1-4410-B5F1-C06B318460B6}");

		/// <summary>
		/// The ID of the New Url field
		/// </summary>
		public static ID NewUrl = new ID("{5BAF3C63-AFCC-4ACE-8268-9092A214A262}");

		/// <summary>
		/// The ID of the Is Permanent field.
		/// </summary>
		public static ID IsPermanent = new ID("{3E6F80BC-E540-4B09-8440-349B24CAB060}");
	}
}
