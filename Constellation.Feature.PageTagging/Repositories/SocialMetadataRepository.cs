using Constellation.Feature.PageTagging.Models;
using Constellation.Foundation.Data;
using Constellation.Foundation.ModelMapping;
using Sitecore.Data.Items;

namespace Constellation.Feature.PageTagging.Repositories
{
	/// <summary>
	/// A repository implementing ISocialMetadataRepository
	/// </summary>
	public class SocialMetadataRepository : ISocialMetadataRepository
	{
		#region Constructor
		/// <summary>
		/// Creates a new instance of SocialMetadataRepository
		/// </summary>
		/// <param name="modelMappper">An instance of IModelMapper, typically provided via Dependency Injection.</param>
		/// <param name="pageMetadataRepository">An instance of IMetadataRepository, typically provided via Dependency Injection.</param>
		public SocialMetadataRepository(IModelMapper modelMappper, IMetadataRepository pageMetadataRepository)
		{
			ModelMapper = modelMappper;
			PageMetadataRepository = pageMetadataRepository;
		}

		#endregion

		#region Properties
		/// <summary>
		/// The instance of IModelMapper to use for transferring values from Items to PageSocialMetadata models.
		/// </summary>
		protected IModelMapper ModelMapper { get; }


		/// <summary>
		/// The instance of IMetadataRepository used for retrieving valid Author and Publisher values.
		/// </summary>
		protected IMetadataRepository PageMetadataRepository { get; }
		#endregion


		/// <inheritdoc />
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

				var candidate = ModelMapper.MapItemToNew<PageSocialMetadata>(context);

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
