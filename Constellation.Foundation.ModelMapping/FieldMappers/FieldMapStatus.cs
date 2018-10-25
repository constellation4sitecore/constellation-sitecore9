namespace Constellation.Foundation.ModelMapping.FieldMappers
{
	/// <summary>
	/// A flag passed from the FieldMapper to the ModelMapper to indicate the result of attempting to map a particular Field Value to a Model Property
	/// </summary>
	public enum FieldMapStatus
	{
		/// <summary>
		/// Property on Model was marked as "Do Not Map."
		/// </summary>
		ExplicitIgnore,
		/// <summary>
		/// Field did not contain a value.
		/// </summary>
		FieldEmpty,
		/// <summary>
		/// There was no Property named after the Field to be mapped.
		/// </summary>
		NoProperty,
		/// <summary>
		/// The Property was not of the expected Type for the given Field Mapper.
		/// </summary>
		TypeMismatch,
		/// <summary>
		/// The Calculated, typecast value of the Field was empty or null.
		/// </summary>
		ValueEmpty,
		/// <summary>
		/// There was a (handled) exception during the mapping of the Field to the Property on the Model.
		/// </summary>
		Exception,
		/// <summary>
		/// The Value of the Field was successfully mapped to the Model Property.
		/// </summary>
		Success
	}
}
