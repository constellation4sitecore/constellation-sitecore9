using Constellation.Feature.PageTagging.Models;
using Constellation.Foundation.Data;
using Constellation.Foundation.ModelMapping;
using Sitecore.Data.Items;

namespace Constellation.Feature.PageTagging
{
	public static class SocialMetadataRepository
	{
		public static PageSocialMetadata GetMetadata(Item contextItem)
		{
			var model = contextItem.MapToNew<PageSocialMetadata>();

			FillSocialMetadata(contextItem, model);

			PageMetadataRepository.FillAuthorAndPublisher(contextItem, model);

			return model;
		}

		private static void FillSocialMetadata(Item context, PageSocialMetadata model)
		{
			while (true)
			{
				if (!model.InheritTwitterValues)
				{
					return; // Model thinks it's OK.
				}

				if (!string.IsNullOrEmpty(model.TwitterCreator) || !string.IsNullOrEmpty(model.TwitterSite))
				{
					return; // we've got enough to go on.
				}

				if (context == null)
				{
					return; // weird. Shouldn't happen, but if it doesn, we're OK.
				}

				if (!context.IsDerivedFrom(PageTaggingTemplateIDs.PageSocialMetadataID))
				{
					return; // Prevents escalating up the tree past /sitecore/content
				}

				var candidate = context.MapToNew<PageSocialMetadata>();

				if (string.IsNullOrEmpty(model.TwitterCreator))
				{
					model.TwitterCreator = candidate.TwitterCreator;
				}

				if (string.IsNullOrEmpty(model.TwitterSite))
				{
					model.TwitterSite = candidate.TwitterSite;
				}

				context = context.Parent;
			}
		}
	}
}
