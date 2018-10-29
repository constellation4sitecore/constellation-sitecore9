using System.Collections.Generic;
using Constellation.Foundation.Caching;
using Constellation.Foundation.ModelMapping;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Constellation.Foundation.Mvc.Patterns.Repositories
{
	/// <summary>
	/// A Repository that generates a ViewModel that is based on a list of Items. This repository should be used when multiple Items need
	/// to be retrieved and mapped to individual ViewModels. The query conditions should be retrievable from the RepositoryContext, typically either
	/// the Rendering Datasource or the Request Item (Sitecore Context Item). This repository will cache the finished model to reduce database requests
	/// </summary>
	/// <typeparam name="TListRecord">The ViewModel to use for individual Items in the result set.</typeparam>
	public abstract class ItemListRepository<TListRecord> : CachingRepository<ICollection<TListRecord>>
	where TListRecord : class, new()
	{
		/// <summary>
		/// Creates a new instance of ItemListRepository
		/// </summary>
		/// <param name="modelMapper">The Mapper to use, usually provided by Dependency Injection.</param>
		/// <param name="cacheManager">The Cache Manager to use, usually provided by Dependency Injection.</param>
		protected ItemListRepository(IModelMapper modelMapper, ICacheManager cacheManager) : base(cacheManager)
		{
			ModelMapper = modelMapper;
		}

		#region Properties
		/// <summary>
		/// The Mapper to use to map Item Field values to ViewModel properties.
		/// </summary>
		protected IModelMapper ModelMapper { get; }
		#endregion

		/// <inheritdoc />
		protected override ICollection<TListRecord> GetUncachedModel(RepositoryContext context)
		{
			var items = GetItems(context);

			if (items == null)
			{
				Log.Warn($"{this.GetType().Name}: Repository GetItems() returned null. Datasource was {context.Datasource.ID}. RequestItem was {context.RequestItem.ID}", this);
				return null;
			}

			return ModelMapper.MapToCollectionOf<TListRecord>(items);
		}

		/// <summary>
		/// Use this method for Sitecore data retrieval. The resulting Items will be mapped to a collection of ViewModels represented by TListRecord.
		/// DO NOT ACCESS THE CACHE FROM THIS METHOD.
		/// Note that the parameters for locating the Item should be available from the Items available in the RepositoryContext.
		/// </summary>
		/// <param name="context">The Context of the data request.</param>
		/// <returns>A collection of Items ready for Mapping and caching.</returns>
		protected abstract ICollection<Item> GetItems(RepositoryContext context);
	}
}
