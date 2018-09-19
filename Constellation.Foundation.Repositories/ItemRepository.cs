using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace Constellation.Foundation.Repositories
{
	public class ItemRepository
	{
		#region Constructors

		public ItemRepository(Item context)
		{
			Database = context.Database;
			Language = context.Language;
		}

		public ItemRepository(Database database, Language language)
		{
			Database = database;
			Language = language;
		}
		#endregion

		#region Properties
		public Database Database { get; private set; }

		public Language Language { get; private set; }
		#endregion

		#region Methods

		public Item GetItem(ID id)
		{
			return Database.GetItem(id, Language);
		}

		public Item SelectSingleItem(string query)
		{
			return Database.SelectSingleItem(query);
		}

		public Item SelectSingleItemUsingXPath(string query)
		{
			return Database.SelectSingleItemUsingXPath(query);
		}

		public Item[] SelectItems(string query)
		{
			return Database.SelectItems(query);
		}

		public ItemList SelectItemsUsingXPath(string query)
		{
			return Database.SelectItemsUsingXPath(query);
		}
		#endregion

	}
}
