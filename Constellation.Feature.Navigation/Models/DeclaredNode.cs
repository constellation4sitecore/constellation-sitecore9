namespace Constellation.Feature.Navigation.Models
{
	/// <summary>
	/// A member of a "static" navigation menu.
	/// </summary>
	public class DeclaredNode
	{
		/// <summary>
		/// Represents the Menu, Link Group or Navigation Link that manages this Node instance.
		/// </summary>
		public DeclaredNode Parent { get; set; }

		private bool _isActive;

		/// <summary>
		/// Indicates that one of the descendant links of this Node points to an Item that is an Ancestor to the
		/// Request's Context Item.
		/// </summary>
		public bool IsActive
		{
			get
			{
				return _isActive;
			}
			set
			{
				_isActive = value;

				if (Parent != null)
				{
					Parent.IsActive = _isActive;
				}
			}
		}
	}
}
