using System.Collections.Generic;
using Constellation.Foundation.Mvc.Patterns.Repositories;
using Sitecore.Data.Items;

namespace Constellation.Foundation.Mvc.Patterns
{
	public abstract class ItemListRepository : IRepository
	{
		public abstract ICollection<Item> GetItems(Item datasource, Item contextItem);
	}
}
