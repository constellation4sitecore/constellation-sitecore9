using Sitecore.Data.Items;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Constellation.Foundation.UrlFriendlyPageNames
{
	/// <summary>
	/// Utility class for generating SEO-friendly names for Items.
	/// </summary>
	public static class ItemNameManager
	{
		#region Enums
		/// <summary>
		/// Used for specifying options when the SeoNameManager encounters a space " " character.
		/// </summary>
		public enum SpaceHandling
		{
			/// <summary>
			/// Replace encountered spaces with the dash "-" character.
			/// </summary>
			UseDash,

			/// <summary>
			/// Remove encountered spaces.
			/// </summary>
			Remove
		}

		/// <summary>
		/// Used for specifying options when the SeoNameManager encounters mixed-case names.
		/// </summary>
		public enum CaseHandling
		{
			/// <summary>
			/// Convert string to lowercase.
			/// </summary>
			ForceLowercase,

			/// <summary>
			/// Leave mixed-casing intact.
			/// </summary>
			AsIs
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Creates a replacement Name value for an Item. The replacement Name is returned in the
		/// uniqueName output parameter.
		/// </summary>
		/// <param name="item">The Item requiring Name replacement.</param>
		/// <param name="disallowed">Regex string of characters that should be stripped from the name.</param>
		/// <param name="removeSpaces">Desired behavior for space replacement in the name.</param>
		/// <param name="forceLowercase">Desired behavior for mixed-case names.</param>
		/// <param name="replaceDiacritics">Desired behavior for handling of diacritics in text.</param>
		/// <param name="uniqueName">The new Name value for the Item.</param>
		/// <returns><c>true</c> if uniqueName is different from the supplied Item.Name.</returns>
		public static bool GetLocallyUniqueItemName(Item item, string disallowed, SpaceHandling removeSpaces, CaseHandling forceLowercase, bool replaceDiacritics, out string uniqueName)
		{
			var newName = GetNewItemName(item.Name, disallowed, removeSpaces, forceLowercase, replaceDiacritics);
			var finalName = newName;

			var nameSuffix = 0;
			while (NameIsTakenBySibling(item, finalName))
			{
				nameSuffix++;
				finalName = newName + "-" + nameSuffix.ToString(CultureInfo.InvariantCulture);
			}

			uniqueName = finalName;
			return true;
		}

		/// <summary>
		/// Simple guard test that prevents protected Items from being renamed as well as 
		/// useless renaming in delivery environments.
		/// </summary>
		/// <param name="item">The Item requiring Name replacement.</param>
		/// <returns><c>true</c> if the Item is unprotected and from the Master database.</returns>
		public static bool IsTargetForRenaming(Item item)
		{
			if (item.Database.Name != "master")
			{
				return false;
			}

			return !item.Appearance.ReadOnly;
		}
		#endregion

		/// <summary>
		/// Actual method where string manipulation takes place.
		/// </summary>
		/// <param name="oldName">The original name of the target Item.</param>
		/// <param name="disallowed">Regex string of characters that should be stripped from the name.</param>
		/// <param name="removeSpaces">Desired behavior for space replacement in the name.</param>
		/// <param name="forceLowercase">Desired behavior for mixed-case names.</param>
		/// <param name="replaceDiacritics">Desired behavior for handling of diacritics in text.</param>
		/// <returns>The modified string to use as the name.</returns>
		public static string GetNewItemName(string oldName, string disallowed, SpaceHandling removeSpaces, CaseHandling forceLowercase, bool replaceDiacritics)
		{
			var spaceReplacer = string.Empty;

			if (removeSpaces == SpaceHandling.UseDash)
			{
				spaceReplacer = "-";
			}

			var name = oldName.Replace(" ", spaceReplacer);
			name = Regex.Replace(name, disallowed, string.Empty);

			if (replaceDiacritics)
			{
				name = ReplaceDiacritics(name);
			}

			return forceLowercase == CaseHandling.ForceLowercase ? name.ToLower(CultureInfo.InvariantCulture) : name;
		}

		#region Helpers
		/// <summary>
		/// Checks that no siblings of the target Item will share the proposed name.
		/// </summary>
		/// <param name="item">The Item requiring Name replacement.</param>
		/// <param name="name">The candidate replacement name.</param>
		/// <returns><c>true</c>if the name already exists in this branch of the content tree.</returns>
		private static bool NameIsTakenBySibling(Item item, string name)
		{
			if (item.Parent == null)
			{
				return false;
			}

			var siblings = item.Parent.GetChildren();

			foreach (Item sibling in siblings)
			{
				if (sibling.ID.ToGuid().Equals(item.ID.ToGuid()))
				{
					continue;
				}

				if (sibling.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Facilitates the replacement of diacritics from a string.
		/// </summary>
		/// <param name="oldName">The original name of the item.</param>
		/// <returns>The modified string.</returns>
		private static string ReplaceDiacritics(string oldName)
		{
			var decomposed = oldName.Normalize(NormalizationForm.FormD);
			var filtered = decomposed.Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);

			return new string(filtered.ToArray());
		}
		#endregion
	}
}
