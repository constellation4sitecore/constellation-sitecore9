using Sitecore.Globalization;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;
using Sitecore.Mvc.Presentation;

namespace Constellation.Foundation.Mvc.Pipelines.RenderRendering
{
	/// <summary>
	/// Addresses a shortcoming in the default Sitecore MVC RenderRendering pipeline where two renderings that share both a controller
	/// and a datasource cannot cache independent results, although they should be able to.
	/// </summary>
	/// <remarks>
	/// An example of this shortcoming would be if you had a rendering that generated navigation based on a starting point in the Datasource
	/// but had another rendering that used fields on the same Datasource Item to display text. Even though there are two Renderings
	/// defined in Sitecore, because they share a controller and a datasource, they will share an HTML Cache Key, even though they will
	/// resolve to discrete views using ConventionController.
	/// 
	/// This adds the ID of the RenderingItem to the cache key, ensuring that each Rendering Item gets its own cache entry, even
	/// if they use the same datasource.
	/// </remarks>
	public class GenerateRenderingSpecificCacheKey : Sitecore.Mvc.Pipelines.Response.RenderRendering.GenerateCacheKey
	{
		/// <inheritdoc />
		public GenerateRenderingSpecificCacheKey(RendererCache rendererCache) : base(rendererCache)
		{
		}

		/// <summary>
		/// Generates the Cache Key for the rendering.
		/// </summary>
		/// <param name="rendering">The Rendering</param>
		/// <param name="args">The Arguments</param>
		/// <returns>The Cache Key</returns>
		protected override string GenerateKey(Rendering rendering, RenderRenderingArgs args)
		{
			var str1 = rendering.Caching.CacheKey.OrIfEmpty(args.Rendering.Renderer.ValueOrDefault(renderer => renderer.CacheKey));
			if (str1.IsEmptyOrNull())
				return null;
			var str2 = str1 + "_#lang:" + Language.Current.Name.ToUpper() + this.GetAreaPart(args);

			str2 += rendering.RenderingItem.ID.ToShortID().ToString();

			RenderingCachingDefinition caching = rendering.Caching;
			if (caching.VaryByData)
				str2 += this.GetDataPart(rendering);
			if (caching.VaryByDevice)
				str2 += this.GetDevicePart(rendering);
			if (caching.VaryByLogin)
				str2 += this.GetLoginPart(rendering);
			if (caching.VaryByUser)
				str2 += this.GetUserPart(rendering);
			if (caching.VaryByParameters)
				str2 += this.GetParametersPart(rendering);
			if (caching.VaryByQueryString)
				str2 += this.GetQueryStringPart(rendering);
			return str2;
		}
	}
}
