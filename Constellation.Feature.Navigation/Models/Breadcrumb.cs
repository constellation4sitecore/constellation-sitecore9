namespace Constellation.Feature.Navigation.Models
{
	/// <summary>
	/// An extension of TargetItem that includes a flag to indicate if this Model represents the Context Item.
	/// </summary>
	public class Breadcrumb : TargetItem
	{
		/// <summary>
		/// Indicates if the Item represented by this model is the Context Item.
		/// </summary>
		public bool IsContextItem { get; set; }
	}
}
