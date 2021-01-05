using Sitecore.Diagnostics;
using Sitecore.Resources.Media;
using System;
using System.IO;

namespace Constellation.Foundation.Images
{
	public class WebPThumbnailGenerator : ThumbnailGenerator
	{
		public override MediaStream GetStream(MediaData mediaData, TransformationOptions options)
		{
			throw new NotImplementedException();
		}

		/// <summary>Gets a stream from an image stream.</summary>
		/// <param name="inputStream">The stream containing the image data.</param>
		/// <param name="options">The options.</param>
		/// <returns>The <see cref="T:System.IO.Stream" />.</returns>
		protected override Stream GetImageStream(Stream inputStream, TransformationOptions options)
		{
			Assert.ArgumentNotNull((object)inputStream, nameof(inputStream));
			Assert.ArgumentNotNull((object)options, nameof(options));

			var output = new WebPImageEffects().TransformImageStream(inputStream, options);

			return output;
		}
	}
}
