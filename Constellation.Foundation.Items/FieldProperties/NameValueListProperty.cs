namespace Constellation.Foundation.Items.FieldProperties
{
	using Sitecore.Data.Fields;

	using System.Collections.Specialized;

	/// <inheritdoc />
	/// <summary>
	/// Wraps a Sitecore NameValueListField
	/// </summary>
	public class NameValueListProperty : FieldProperty
	{
		/// <summary>
		/// The field to wrap.
		/// </summary>
		private readonly NameValueListField _nameValueListField;

		#region Constructors
		/// <inheritdoc />
		/// <summary>
		/// Initializes a new instance of the <see cref="NameValueListProperty"/> class.
		/// </summary>
		/// <param name="field">The field to wrap.</param>
		public NameValueListProperty(Field field)
			: base(field)
		{
			this._nameValueListField = field;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Sitecore.Data.Fields.CheckboxField"/> is checked.
		/// </summary>
		/// <value>
		/// <c>true</c> if checked; otherwise, <c>false</c>.
		/// </value>
		public NameValueCollection NameValues
		{
			get { return this._nameValueListField.NameValues; }
			set { this._nameValueListField.NameValues = value; }
		}
		#endregion

		#region Operators
		/// <summary>
		/// Allows interoperability with Sitecore NameValueListField.
		/// </summary>
		/// <param name="field">The field to wrap.</param>
		/// <returns>A new instance of NameValueListProperty using the supplied field.</returns>
		public static implicit operator NameValueListProperty(NameValueListField field)
		{
			return new NameValueListProperty(field.InnerField);
		}

		/// <summary>
		/// Allows interoperability with Sitecore ImageField.
		/// </summary>
		/// <param name="property">The property to convert.</param>
		/// <returns>The property.InnerField.</returns>
		public static implicit operator NameValueListField(NameValueListProperty property)
		{
			return property.InnerField;
		}
		#endregion
	}
}
