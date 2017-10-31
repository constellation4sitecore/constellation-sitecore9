using Sitecore.Data.Items;
using Sitecore.Sites;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Constellation.Feature.SitemapXml
{
	/// <inheritdoc />
	/// <summary>
	/// Represents a candidate element for the sitemap.xml file.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Implementers that use strongly-typed Item frameworks like CustomItem or Glass should use this class as a
	/// base for their own SitemapNode implementation.
	/// </para>
	/// <para>
	/// "T" should be a strongly-typed Item type that is suitable for all Items that are at or below
	/// the "home" Items of all sites in the instance, - typically a base class for whatever framework you're using.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">An object representing a Sitecore Item.</typeparam>
	public abstract class SitemapNode<T> : ISitemapNode
	{
		#region Fields
		/// <summary>
		/// Internal storage for the source Item.
		/// </summary>
		private readonly Item item;

		/// <summary>
		/// Internal storage for the Change Frequency.
		/// </summary>
		private ChangeFrequency changeFrequency = ChangeFrequency.Monthly;

		/// <summary>
		/// Internal storage for the Is Page flag.
		/// </summary>
		[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
		private bool isPage;

		/// <summary>
		/// Internal storage for the Is Listed in Navigation flag.
		/// </summary>
		[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
		private bool isListedInNavigation;

		/// <summary>
		/// Determines whether the item has been parsed for this instance.
		/// </summary>
		private bool initialized;

		/// <summary>
		/// Internal storage for the seeding object.
		/// </summary>
		private T itemObject;

		/// <summary>
		/// Internal storage for the page's URL.
		/// </summary>
		private string location = string.Empty;

		/// <summary>
		/// Internal storage for the Should Index flag.
		/// </summary>
		private bool shouldIndex = true;

		/// <summary>
		/// Internal storage for Last Modified.
		/// Default value is SqlDateTime.MinValue.
		/// </summary>
		[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
		private DateTime lastModified = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;

		/// <summary>
		/// Internal storage for priority.
		/// </summary>
		private decimal priority;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="SitemapNode&lt;T&gt;"/> class based on
		/// the Item supplied.
		/// </summary>
		/// <param name="item">
		/// The Sitecore Item to interrogate.
		/// </param>
		/// <param name="site">
		/// The site.
		/// </param>
		protected SitemapNode(Item item, SiteContext site)
		{
			this.item = item;
			this.Site = site;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the expected frequency of content updates for this page.
		/// </summary>
		public ChangeFrequency ChangeFrequency
		{
			get
			{
				this.Initialize();
				return this.changeFrequency;
			}
		}

		/// <summary>
		/// Gets the site.
		/// </summary>
		public SiteContext Site { get; private set; }

		/// <summary>
		/// Gets a value indicating whether the page is listed in site navigation.
		/// </summary>
		public bool IsListedInNavigation
		{
			get
			{
				this.Initialize();
				return this.isListedInNavigation;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this node is a page.
		/// Default is false.
		/// </summary>
		public bool IsPage
		{
			get
			{
				this.Initialize();
				return this.isPage;
			}
		}

		/// <summary>
		/// Gets the absolute URL of this node including protocol.
		/// </summary>
		public string Location
		{
			get
			{
				this.Initialize();
				return this.location;
			}
		}

		/// <summary>
		/// Gets the indexing priority of this node on a scale from 0.0 to 1.0.
		/// 0.0 is lowest priority.
		/// 1.0 is highest priority.
		/// neutral is 0.5.
		/// </summary>
		public decimal Priority
		{
			get
			{
				this.Initialize();
				return this.priority;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the page should be indexed by search engines.
		/// Default is true.
		/// </summary>
		public bool ShouldIndex
		{
			get
			{
				this.Initialize();
				return this.shouldIndex;
			}
		}

		/// <summary>
		/// Gets the date on which the content of this page was modified.
		/// Default is SqlDateTime.MinValue.
		/// </summary>
		[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
		public DateTime LastModified
		{
			get
			{
				this.Initialize();
				return this.lastModified;
			}
		}
		#endregion

		#region Page Attribute Detection
		/// <summary>
		/// Determines whether the item object (which is assumed to be a page) is to be represented
		/// in navigation presented on page (ex: a sitemap or breadcrumbs).
		/// </summary>
		/// <param name="item">An object representing a Sitecore Item.</param>
		/// <returns><c>true</c> if the object is intended to be represented in various page navigation scenarios.</returns>
		protected abstract bool CheckIsListedInNavigation(T item);

		/// <summary>
		/// Determines whether the item object represents a web page on the site and therefore
		/// has presentation details.
		/// </summary>
		/// <param name="item">An object representing a Sitecore Item.</param>
		/// <returns><c>true</c> if the object is a page with presentation details given the current context.</returns>
		protected abstract bool CheckIsPage(T item);

		/// <summary>
		/// Determines whether the item object (which is assumed to be a page) is to be indexed
		/// by search engines.
		/// </summary>
		/// <param name="item">An object representing a Sitecore Item.</param>
		/// <returns><c>true</c> if the object's presentation is intended to be crawled by search engines.</returns>
		protected abstract bool CheckShouldIndex(T item);

		/// <summary>
		/// Determines the full URL (hostname &amp; language definition) of the item object (which is assumed to be a page).
		/// </summary>
		/// <param name="item">An object representing a Sitecore Item.</param>
		/// <returns>The absolute URL to the object, including protocol.</returns>
		protected abstract string ResolveAbsoluteUrl(T item);

		/// <summary>
		/// Determines the anticipated content/presentation change frequency of the item object (which is assumed to be a page).
		/// </summary>
		/// <param name="item">An object representing a Sitecore Item.</param>
		/// <returns>The change frequency.</returns>
		protected abstract ChangeFrequency ResolveChangeFrequency(T item);

		/// <summary>
		/// Determines the crawling priority of the item object (which is assumed to be a page) using a scale
		/// from 0.0 to 1.0 where 1.0 is the highest priority and 0.0 is the lowest priority. 
		/// 0.5 is the expected neutral value.
		/// </summary>
		/// <param name="item">An object representing a Sitecore Item.</param>
		/// <returns>The numeric prioritization value.</returns>
		protected abstract decimal ResolvePriority(T item);

		/// <summary>
		/// Determines the last date that the item object was modified.
		/// </summary>
		/// <param name="item">An object representing a Sitecore Item.</param>
		/// <returns>The last date of modification.</returns>
		protected abstract DateTime ResolveUpdatedDate(T item);
		#endregion

		#region Item conversion
		/// <summary>
		/// Convert the Item into the strongly-typed representation of the Item.
		/// </summary>
		/// <param name="item">The Item to convert.</param>
		/// <returns>An instance of T.</returns>
		protected abstract T Convert(Item item);
		#endregion

		#region Private Methods
		/// <summary>
		/// Resolves all internal variables.
		/// </summary>
		private void Initialize()
		{
			if (!this.initialized)
			{
				this.itemObject = this.Convert(this.item);
				this.location = this.ResolveAbsoluteUrl(this.itemObject);
				this.lastModified = this.ResolveUpdatedDate(this.itemObject);
				this.isPage = this.CheckIsPage(this.itemObject);
				this.isListedInNavigation = this.CheckIsListedInNavigation(this.itemObject);
				this.shouldIndex = this.CheckShouldIndex(this.itemObject);
				this.changeFrequency = this.ResolveChangeFrequency(this.itemObject);
				this.priority = this.ResolvePriority(this.itemObject);
				this.initialized = true;
			}
		}
		#endregion
	}
}
