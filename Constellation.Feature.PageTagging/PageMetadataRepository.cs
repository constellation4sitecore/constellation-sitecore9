using Constellation.Feature.PageTagging.Models;
using Constellation.Foundation.Data;
using Constellation.Foundation.ModelMapping;
using Sitecore.Data.Items;

namespace Constellation.Feature.PageTagging
{
	public static class PageMetadataRepository
	{
		public static PageMetadata GetMetadata(Item contextItem)
		{
			var model = contextItem.MapToNew<PageMetadata>();

			FillAuthorAndPublisher(contextItem, model);

			return model;
		}

		public static void FillAuthorAndPublisher(Item context, PageMetadata model)
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
