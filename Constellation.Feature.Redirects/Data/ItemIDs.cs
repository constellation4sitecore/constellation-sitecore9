using Sitecore.Data;

namespace Constellation.Feature.Redirects.Data
{
	/// <summary>
	/// IDs of system-critical Items
	/// </summary>
	public static class ItemIDs
	{
		/// <summary>
		/// The ID of the Bucket Item that houses all Marketing Redirects
		/// </summary>
		public static ID MarketingRedirectBucketID = new ID("{07FDE145-14DF-4898-A131-371DBFA2C6BE}");

		/// <summary>
		/// The ID of the Template used for creating new Marketing Redirects
		/// </summary>
		public static ID MarketingRedirectTemplateID = new ID("{3D3BFF42-2BB6-411C-B01C-B8664F9496EF}");
	}
}
