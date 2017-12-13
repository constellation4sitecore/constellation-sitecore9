using System.Collections.Generic;

namespace Constellation.Feature.Navigation.Models
{
	public class NavigationNode : TargetItem
	{
		public ICollection<NavigationNode> Children { get; set; }

		public bool IsActive { get; set; }
	}
}
