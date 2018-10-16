using Constellation.Feature.PageTagging.Models;
using Constellation.Foundation.Data;
using Constellation.Foundation.ModelMapping;
using Sitecore.Data.Items;

namespace Constellation.Feature.PageTagging.Repositories
{
	public class SocialMetadataRepository : ISocialMetadataRepository
	{
		#region Constructor

		public SocialMetadataRepository(IModelMapper modelMappper, IMetadataRepository pageMetadataRepository)
		{
			ModelMapper = modelMappper;
			PageMetadataRepository = pageMetadataRepository;
		}

		#endregion

		#region Properties
		protected IModelMapper ModelMapper { get; }


		protected IMetadataRepository PageMetadataRepository { get; }
		#endregion

		public PageSocialMetadata GetMetadata(Item contextItem)
		{
			var model = ModelMapper.MapItemToNew<PageSocialMetadata>(contextItem);

			FillSocialMetadata(contextItem, model);

			PageMetadataRepository.FillAuthorAndPublisher(contextItem, model);

			return model;
		}

		private void FillSocialMetadata(Item context, PageSocialMetadata model)
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
					return; // weird. Shouldn't happen, but if it does, we're OK.
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
