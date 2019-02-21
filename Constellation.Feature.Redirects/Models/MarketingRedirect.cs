using System.Collections.Generic;
using System.ComponentModel;
using Constellation.Foundation.ModelMapping.MappingAttributes;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Converters;
using Sitecore.Data;

namespace Constellation.Feature.Redirects.Models
{
	/// <summary>
	/// An Item that represents a Marketing Url Redirect
	/// </summary>
	public class MarketingRedirect
	{
		#region The only relevant SearchResultItem fields we need.
		/// <summary>
		/// The ID of the Sitecore Item.
		/// </summary>
		[IndexField("_group")]
		[TypeConverter(typeof(IndexFieldIDValueConverter))]
		public virtual ID ItemId { get; set; }

		/// <summary>
		/// The Item Name
		/// </summary>
		[IndexField("_name")]
		public virtual string Name { get; set; }

		/// <summary>
		/// The name of the database where this particular Item record was sourced
		/// </summary>
		[IndexField("_database")]
		public virtual string DatabaseName { get; set; }

		/// <summary>
		/// The language variant from which this particular search record was sourced
		/// </summary>
		[IndexField("_language")]
		public virtual string Language { get; set; }

		/// <summary>
		/// The ID of the Template used to create the source Item.
		/// </summary>
		[IndexField("_template")]
		[TypeConverter(typeof(IndexFieldIDValueConverter))]
		public virtual ID TemplateId { get; set; }

		/// <summary>
		/// A collection of IDs for ancestors to the Item represented.
		/// </summary>
		[IndexField("_path")]
		[TypeConverter(typeof(IndexFieldEnumerableConverter))]
		public IEnumerable<ID> Paths { get; set; }
		#endregion

		/// <summary>
		/// The name of the Site as it appears in the Sitecore config files.
		/// </summary>
		[IndexField("site_name")]
		[RawValueOnly]
		public virtual string SiteName { get; set; }

		/// <summary>
		/// The Local-path URL that should be redirected
		/// </summary>
		[IndexField("old_url")]
		[RawValueOnly]
		public virtual string OldUrl { get; set; }

		/// <summary>
		/// The target URL of the redirect
		/// </summary>
		[IndexField("new_url")]
		[RawValueOnly]
		public virtual string NewUrl { get; set; }

		/// <summary>
		/// Specifies that this particular redirect should issue a Permanent Redirect header.
		/// </summary>
		[IndexField("is_permanent")]
		public virtual bool IsPermanent { get; set; }
	}
}
