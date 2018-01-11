using Constellation.Foundation.ModelMapping.MappingAttributes;
using System.Collections.Generic;

namespace Constellation.Feature.Navigation.Models
{
	/// <summary>
	/// Represents a navigation menu option in HTML that has a destination.
	/// </summary>
	public class NavigationLink
	{
		public NavigationLink()
		{
			ChildLinks = new List<NavigationLink>();
		}

		/// <summary>
		/// The link represented by this Navigation Link instance. Can be an internal,
		/// external, or media item link.
		/// </summary>
		[RenderAsUrl(false)]
		public string Link { get; set; }

		/// <summary>
		/// The Class value of the General Link field of this Navigation Link Item. Represents the CSS class to apply to the anchor tag.
		/// </summary>
		public string LinkClass { get; set; }

		/// <summary>
		/// The Title value of the General Link field of this Navigation Link Item. Represents the anchor link's title attribute.
		/// </summary>
		public string LinkTitle { get; set; }

		/// <summary>
		/// The Text value of the General Link field of this Navigation Link Item.
		/// </summary>
		public string LinkText { get; set; }

		/// <summary>
		/// The Target value of the General LInk field of this Navigation Link Item. Represents the anchor link's target attribute.
		/// </summary>
		public string LinkTarget { get; set; }

		/// <summary>
		/// If the Link field's target is Internal, this property will contain essential information about the target to allow its properties to be rendered on page.
		/// Note that if the target Item is not published, this value will be null.
		/// </summary>
		public TargetItem LinkTargetItem { get; set; }

		/// <summary>
		/// Flag set by content authors indicating that all other possible text values for this navigation link should be superseded by the value of the Navigation Link Item's DisplayName.
		/// </summary>
		public bool UseThisDisplayName { get; set; }

		/// <summary>
		/// The DisplayName of the current Navigation Link Item.
		/// </summary>
		public string DisplayName { get; set; }

		/// <summary>
		/// Any Navigation Link Items that are children of the current Navigation Link Item.
		/// </summary>
		public ICollection<NavigationLink> ChildLinks { get; set; }

		public string GetBestLinkText()
		{
			if (UseThisDisplayName)
			{
				return DisplayName;

			}

			if (!string.IsNullOrEmpty(LinkText))
			{
				return LinkText;
			}

			if (!string.IsNullOrEmpty(LinkTargetItem?.NavigationTitle))
			{
				return LinkTargetItem.NavigationTitle;
			}

			if (!string.IsNullOrEmpty(LinkTargetItem?.DisplayName))
			{
				return LinkTargetItem.DisplayName;
			}

			return DisplayName;
		}
	}
}
