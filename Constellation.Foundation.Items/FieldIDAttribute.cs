namespace Constellation.Foundation.Items
{
	using Sitecore.Data;

	using System;

	/// <inheritdoc />
	/// <summary>
	/// Specifies what Sitecore Field a given property represents.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	// ReSharper disable InconsistentNaming
	public class FieldIDAttribute : Attribute
	// ReSharper restore InconsistentNaming
	{
		#region Constructors
		/// <inheritdoc />
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Constellation.Foundation.Items.FieldIDAttribute" /> class.
		/// </summary>
		/// <param name="id">The ID of the Field.</param>
		public FieldIDAttribute(ID id)
		{
			ID = id;
		}

		/// <inheritdoc />
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Constellation.Foundation.Items.FieldIDAttribute" /> class.
		/// </summary>
		/// <param name="id">The ID of the Field as a Guid.</param>
		public FieldIDAttribute(Guid id)
		{
			ID = new ID(id);
		}

		/// <inheritdoc />
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Constellation.Foundation.Items.FieldIDAttribute" /> class.
		/// </summary>
		/// <param name="id">The ID of the Field as a String.</param>
		public FieldIDAttribute(string id)
		{
			ID = new ID(id);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets The ID of the Field.
		/// </summary>
		// ReSharper disable once InconsistentNaming
		public ID ID { get; }
		#endregion

	}
}
