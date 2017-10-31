namespace Constellation.Foundation.Items.FieldProperties
{
	using Sitecore.Data.Fields;
	using Sitecore.Web.UI.WebControls;

	/// <inheritdoc />
	/// <summary>
	/// Wraps a Sitecore TextField.
	/// </summary>
	public class TextProperty : FieldProperty
	{
		#region Constructors
		/// <inheritdoc />
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Constellation.Foundation.Items.FieldProperties.TextProperty" /> class.
		/// </summary>
		/// <param name="field">The field to wrap.</param>
		public TextProperty(Field field)
			: base(field)
		{
			// Nothing to do.
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the FieldRenderer output for this property.
		/// </summary>
		public string Text
		{
			get { return FieldRenderer.Render(InnerField.Item, this.Name); }
		}
		#endregion

		#region Operators
		/// <summary>
		/// Allows interoperability with string.
		/// </summary>
		/// <param name="property">The property to convert.</param>
		/// <returns>The property.Text value.</returns>
		public static implicit operator string(TextProperty property)
		{
			return property.Text;
		}

		/// <summary>
		/// Allows interoperability with Sitecore TextField.
		/// </summary>
		/// <param name="field">The field to wrap.</param>
		/// <returns>A new instance of TextProperty using the supplied field.</returns>
		public static implicit operator TextProperty(TextField field)
		{
			return new TextProperty(field.InnerField);
		}

		/// <summary>
		/// Allows interoperability with Sitecore TextField.
		/// </summary>
		/// <param name="property">The property to convert.</param>
		/// <returns>The property.InnerField.</returns>
		public static implicit operator TextField(TextProperty property)
		{
			return property.InnerField;
		}
		#endregion

		#region Methods

		/// <summary>
		/// Returns the value of the Text property, which is Page Editor compatible.
		/// </summary>
		/// <returns>
		/// The <see cref="string"/>.
		/// </returns>
		public override string ToString()
		{
			return this.Text;
		}
		#endregion
	}
}
