using System.Collections.Generic;

namespace Constellation.Feature.StaticNavigation.Models
{
	public class SimpleNavigation
	{
		public SimpleNavigation()
		{
			Links = new List<NavigationLink>();
		}

		public string DisplayName { get; set; }

		public ICollection<NavigationLink> Links { get; }
	}
}
