namespace Constellation.Feature.SitemapXml
{
	/// <summary>
	/// Indicates to search engines how often a page should be indexed
	/// by specifying the frequency of page changes.
	/// </summary>
	public enum ChangeFrequency
	{
		/// <summary>
		/// Used to describe documents that change each time they are accessed.
		/// </summary>
		Always,

		/// <summary>
		/// Used to describe documents that change every hour.
		/// </summary>
		Hourly,

		/// <summary>
		/// Used to describe documents that change every day.
		/// </summary>
		Daily,

		/// <summary>
		/// Used to describe documents that change every week.
		/// </summary>
		Weekly,

		/// <summary>
		/// Used to describe documents that change every month.
		/// </summary>
		Monthly,

		/// <summary>
		/// Used to describe documents that change every year.
		/// </summary>
		Yearly,

		/// <summary>
		/// Used to describe archived URLs.
		/// </summary>
		Never
	}
}
