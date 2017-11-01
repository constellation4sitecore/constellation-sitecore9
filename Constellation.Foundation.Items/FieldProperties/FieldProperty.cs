namespace Constellation.Foundation.Items.FieldProperties
{
	using Sitecore.Data;
	using Sitecore.Data.Fields;
	using Sitecore.Data.Items;
	using Sitecore.Data.Templates;
	using Sitecore.Globalization;

	/// <summary>
	/// Provides a facade for Sitecore CustomField.
	/// </summary>
	public class FieldProperty : CustomField
	{
		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="FieldProperty"/> class.
		/// </summary>
		/// <param name="field">The field to wrap.</param>
		public FieldProperty(Field field)
			: base(field)
		{
			// Nothing to do Stylecop.
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets a value indicating whether the field can be read in the current context.
		/// </summary>
		public bool CanRead => InnerField.CanRead;

		/// <summary>
		/// Gets a value indicating whether the field can be written in the current context.
		/// </summary>
		public bool CanWrite => InnerField.CanWrite;

		/// <summary>
		/// Gets a value indicating whether the field's value is a Standard Value.
		/// </summary>
		public bool ContainsStandardValue => InnerField.ContainsStandardValue;

		/// <summary>
		/// Gets a value indicating whether the field's value is inherited from another Item.
		/// </summary>
		public bool InheritsValueFromOtherItem => InnerField.InheritsValueFromOtherItem;

		/// <summary>
		/// Gets the Database for the field.
		/// </summary>
		public Database Database => InnerField.Database;

		/// <summary>
		/// Gets the field definition.
		/// </summary>
		public TemplateField Definition => InnerField.Definition;

		/// <summary>
		/// Gets the field description.
		/// </summary>
		public string Description => InnerField.Description;

		/// <summary>
		/// Gets the field display name. (Includes field Title.)
		/// </summary>
		public string DisplayName => InnerField.DisplayName;

		/// <summary>
		/// Gets the field's Section Name.
		/// </summary>
		public string SectionDisplayName => InnerField.SectionDisplayName;

		/// <summary>
		/// Gets a value indicating whether the field has a blob stream.
		/// </summary>
		public bool HasBlobStream => InnerField.HasBlobStream;

		/// <summary>
		/// Gets a value indicating whether the field's value is not null or empty.
		/// </summary>
		public virtual bool HasValue
		{
			get
			{
				var value = InnerField.HasValue;

				if (!value)
				{
					value = !string.IsNullOrEmpty(InnerField.InheritedValue);
				}

				return value;
			}
		}

		/// <summary>
		/// Gets the URL for external help text.
		/// </summary>
		public string HelpLink => InnerField.HelpLink;

		/// <summary>
		/// Gets the field's ID.
		/// </summary>
		// ReSharper disable InconsistentNaming
		public ID ID => InnerField.ID;
		// ReSharper restore InconsistentNaming


		/// <summary>
		/// Gets the field's inherited value.
		/// </summary>
		public string InheritedValue => InnerField.InheritedValue;

		/// <summary>
		/// Gets a value indicating whether the field is a Blob.
		/// </summary>
		public bool IsBlobField => InnerField.IsBlobField;

		/// <summary>
		/// Gets a value indicating whether the field's value has been modified for this instance.
		/// </summary>
		public bool IsModified => InnerField.IsModified;

		/// <summary>
		/// Gets the Item that the field belongs to.
		/// </summary>
		public Item Item => InnerField.Item;

		/// <summary>
		/// Gets the field Item's Key.
		/// </summary>
		public string Key => InnerField.Key;

		/// <summary>
		/// Gets the field Item's Language.
		/// </summary>
		public Language Language => InnerField.Language;

		/// <summary>
		/// Gets the field Item's Name (also used as an index in the Item's Fields collection).
		/// </summary>
		public string Name => InnerField.Name;

		/// <summary>
		/// Gets a value indicating whether the field has been reset to blank.
		/// </summary>
		public bool ResetBlank => InnerField.ResetBlank;

		/// <summary>
		/// Gets the Field's Section Item.
		/// </summary>
		public string Section => InnerField.Section;

		/// <summary>
		/// Gets the translated name of the field's section.
		/// </summary>
		// ReSharper disable InconsistentNaming
		public string SectionNameByUILocale => InnerField.SectionNameByUILocale;
		// ReSharper restore InconsistentNaming

		/// <summary>
		/// Gets the Sort Order for the field's section.
		/// </summary>
		public int SectionSortorder => InnerField.SectionSortorder;

		/// <summary>
		/// Gets a value indicating whether the field's value is shared across languages.
		/// </summary>
		public bool Shared => InnerField.Shared;

		/// <summary>
		/// Gets a value indicating whether the field's value should be translated.
		/// </summary>
		public bool ShouldBeTranslated => InnerField.ShouldBeTranslated;

		/// <summary>
		/// Gets the field's Sort Order used within the scope of the Field's Section.
		/// </summary>
		public int Sortorder => InnerField.Sortorder;

		/// <summary>
		/// Gets the field's data source.
		/// </summary>
		public string Source => InnerField.Source;

		/// <summary>
		/// Gets the field's CSS settings, used in the Content Editor.
		/// </summary>
		public string Style => InnerField.Style;

		/// <summary>
		/// Gets the field's Title. (Rarely used.)
		/// </summary>
		public string Title => InnerField.Title;

		/// <summary>
		/// Gets the field's ToolTip. (Field's Long Description help text.)
		/// </summary>
		public string ToolTip => InnerField.ToolTip;

		/// <summary>
		/// Gets a value indicating whether the field's value is translatable.
		/// </summary>
		public bool Translatable => InnerField.Translatable;

		/// <summary>
		/// Gets the Sitecore field "type", which is a string.
		/// </summary>
		public string Type => InnerField.Type;

		/// <summary>
		/// Gets the field type key (believed to be the lowercase name of the Type).
		/// </summary>
		public string TypeKey => InnerField.TypeKey;

		/// <summary>
		/// Gets a value indicating whether the field's value applies to all versions of the Item 
		/// for a given language.
		/// </summary>
		public bool Unversioned => InnerField.Unversioned;

		/// <summary>
		/// Gets a RegEx string for old-style validation.
		/// </summary>
		public string Validation => InnerField.Validation;

		/// <summary>
		/// Gets a validation message applying to old-style RegEx validation.
		/// </summary>
		public string ValidationText => InnerField.ValidationText;

		#endregion

		#region Operators
		/// <summary>
		/// Allows for interoperability with Sitecore CustomFields.
		/// </summary>
		/// <param name="field">The Field to wrap.</param>
		/// <returns>A new instance of FieldProperty based on the supplied field.</returns>
		public static implicit operator FieldProperty(Field field)
		{
			return new FieldProperty(field);
		}

		/// <summary>
		/// Allows for interoperability with Sitecore CustomFields.
		/// </summary>
		/// <param name="field">The property to convert.</param>
		/// <returns>The property.InnerField.</returns>
		public static implicit operator Field(FieldProperty field)
		{
			return field.InnerField;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Removes the current value from the field.
		/// </summary>
		public void Clear()
		{
			_innerField.Value = string.Empty;
		}

		/// <summary>
		/// Updates the value of the field.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="force">Normally, if the value is the same 
		/// as the current value, the field is not updated. 
		/// Force causes the field to be marked
		/// as updated anyway.</param>
		public void SetValue(string value, bool force)
		{
			_innerField.SetValue(value, force);
		}
		#endregion
	}
}
