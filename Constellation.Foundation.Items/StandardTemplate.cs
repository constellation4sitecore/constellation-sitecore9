namespace Constellation.Foundation.Items
{
	using Sitecore.Data;
	using Sitecore.Data.Fields;
	using Sitecore.Data.Items;
	using Sitecore.Globalization;

	/// <inheritdoc />
	/// <summary>
	/// Represents a Sitecore Item that descends from the Standard Template.
	/// </summary>
	[TemplateID("{1930BBEB-7805-471A-A3BE-4858AC7CF696}")]
	public class StandardTemplate : CustomItem, IStandardTemplate
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="StandardTemplate"/> class.
		/// </summary>
		/// <param name="item">The Item to wrap.</param>
		public StandardTemplate(Item item)
			: base(item)
		{
			// Nothing to do.
		}
		#endregion

		#region Properties
		/// <inheritdoc />
		/// <summary>
		/// Gets a value indicating whether the Item (version) is empty.
		/// </summary>
		public bool Empty
		{
			get { return InnerItem.Empty; }
		}

		/// <inheritdoc />
		/// <summary>
		/// Gets a value indicating whether the Item has child Items.
		/// </summary>
		public bool HasChildren
		{
			get { return InnerItem.HasChildren; }
		}

		/// <inheritdoc />
		/// <summary>
		/// Gets a value indicating whether the Item has any clones.
		/// </summary>
		public bool HasClones
		{
			get { return InnerItem.HasClones; }
		}

		/// <inheritdoc />
		/// <summary>
		/// Gets the help object associated with this Item.
		/// </summary>
		public ItemHelp Help
		{
			get { return InnerItem.Help; }
		}

		/// <inheritdoc />
		/// <summary>
		/// Gets a value indicating whether the Item is a clone.
		/// </summary>
		public bool IsClone
		{
			get { return InnerItem.IsClone; }
		}

		/// <inheritdoc />
		/// <summary>
		/// Gets a value indicating whether the Item is an Item Clone.
		/// </summary>
		public bool IsItemClone
		{
			get { return InnerItem.IsItemClone; }
		}

		/// <inheritdoc />
		/// <summary>
		/// Gets the Item's Key (lowercased Item.Name).
		/// </summary>
		public string Key
		{
			get { return InnerItem.Key; }
		}

		/// <inheritdoc />
		/// <summary>
		/// Gets the current Item Version's Language.
		/// </summary>
		public Language Language
		{
			get { return InnerItem.Language; }
		}

		/// <inheritdoc />
		/// <summary>
		/// Gets a value indicating whether the Item has been modified.
		/// </summary>
		public bool Modified
		{
			get { return InnerItem.Modified; }
		}

		/// <inheritdoc />
		/// <summary>
		/// Gets the ID of the Item's Parent.
		/// </summary>
		public ID ParentID
		{
			get { return InnerItem.ParentID; }
		}

		/// <inheritdoc />
		/// <summary>
		/// Gets the ID of the Item's Data Template.
		/// </summary>
		public ID TemplateID
		{
			get { return InnerItem.TemplateID; }
		}

		/// <inheritdoc />
		/// <summary>
		/// Gets the name of the Item's Data Template.
		/// </summary>
		public string TemplateName
		{
			get { return InnerItem.TemplateName; }
		}

		/// <inheritdoc />
		/// <summary>
		/// Gets the Item's Version.
		/// </summary>
		public Version Version
		{
			get { return InnerItem.Version; }
		}
		#endregion

		#region Operators
		/// <summary>
		/// Provides compatibility with Sitecore Item.
		/// </summary>
		/// <param name="item">The Item to convert.</param>
		/// <returns>A new instance of IStandardTemplateItem.</returns>
		public static implicit operator StandardTemplate(Item item)
		{
			return ItemFactory.GetStronglyTypedItem(item);
		}

		/// <summary>
		/// Provides compatibility with Sitecore Item.
		/// </summary>
		/// <param name="item">The StandardTemplateItem to convert.</param>
		/// <returns>The InnerItem.</returns>
		public static implicit operator Item(StandardTemplate item)
		{
			return item.InnerItem;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Shorthand for accessing a Field from the inner item.
		/// </summary>
		/// <param name="name">The Name of the field.</param>
		/// <returns>The field with the matching name or null.</returns>
		protected Field GetField(string name)
		{
			return this.InnerItem.Fields[name];
		}

		/// <summary>
		/// Shorthand for accessing a Field from the inner item.
		/// </summary>
		/// <param name="id">The ID of the field.</param>
		/// <returns>The field with the matching ID or null.</returns>
		protected Field GetField(ID id)
		{
			return this.InnerItem.Fields[id];
		}
		#endregion
	}
}
