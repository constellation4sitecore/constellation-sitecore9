using Constellation.Feature.PageTagging.Models;
using Constellation.Foundation.Data;
using Constellation.Foundation.ModelMapping;
using Sitecore.Data.Items;

namespace Constellation.Feature.PageTagging.Repositories
{
	/// <summary>
	/// An implementation of IMetadataRepository
	/// </summary>
	public class MetadataRepository : IMetadataRepository
	{
		#region Constructor
		/// <summary>
		/// Creates a new instance of MetadataRepository
		/// </summary>
		/// <param name="modelMapper">An instance of IModelMapper, typically provided via dependency injection.</param>
		public MetadataRepository(IModelMapper modelMapper)
		{
			ModelMapper = modelMapper;
		}
		#endregion

		#region Properties
		/// <summary>
		/// The ModelMapper to use for mapping Items to the PageMetadata model.
		/// </summary>
		protected IModelMapper ModelMapper { get; }
		#endregion

		/// <inheritdoc />
		public PageMetadata GetMetadata(Item contextItem)
		{
			var model = ModelMapper.MapItemToNew<PageMetadata>(contextItem);

			FillAuthorAndPublisher(contextItem, model);

			return model;
		}

		/// <inheritdoc />
		public void FillAuthorAndPublisher(Item context, PageMetadata model)
		{
			while (true)
			{
				if (model.HasValidAuthorAndPublisher)
				{
					return; // model is now OK from the last round.
				}

				if (context == null)
				{
					return; // weird. Shouldn't happen, but if it does, we're OK.
				}

				if (!context.IsDerivedFrom(PageTaggingTemplateIDs.PageMetadataID))
				{
					return; // Prevents escalating up the tree past /sitecore/content
				}

				var candidate = context.MapToNew<PageMetadata>();

				if (candidate.HasValidAuthorAndPublisher)
				{
					if (!model.HasValidAuthor)
					{
						model.MetaAuthor = candidate.MetaAuthor;
					}
					if (!model.HasValidPublisher)
					{
						model.MetaPublisher = candidate.MetaPublisher;
					}

					return; // We just successfully set one or both properties and can exit.
				}

				if (candidate.HasValidPublisher && !model.HasValidPublisher)
				{
					model.MetaPublisher = candidate.MetaPublisher;
				}

				if (candidate.HasValidAuthor && !model.HasValidAuthor)
				{
					model.MetaAuthor = candidate.MetaAuthor;
				}

				context = context.Parent;
			}
		}
	}
}
