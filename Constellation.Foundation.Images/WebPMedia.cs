using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links.UrlBuilders;
using Sitecore.Pipelines;
using Sitecore.Resources.Media;
using Sitecore.SecurityModel;
using System;
using System.Drawing;
using System.IO;

namespace Constellation.Foundation.Images
{
	public class WebPMedia : ImageMedia
	{
		public override MediaStream GetStream()
		{
			return base.GetStream();
		}

		public override void SetStream(MediaStream mediaStream)
		{
			base.SetStream(mediaStream);
		}

		public override void SetStream(Stream stream, string extension)
		{
			base.SetStream(stream, extension);
		}

		public override void SetImage(Image image)
		{
			base.SetImage(image);
		}

		/// <summary>Gets the media data as an image.</summary>
		/// <returns>
		/// The <see cref="T:System.Drawing.Image" />.
		/// </returns>
		public override Image GetImage()
		{
			MediaStream stream = this.GetStream();
			if (stream == null)
				return (Image)null;
			return stream.Stream.Length == 0L ? (Image)null : Image.FromStream(stream.Stream);
		}


		/// <summary>Updates the meta data of a media item.</summary>
		/// <param name="mediaStream">The media Stream.</param>
		public override void UpdateMetaData(MediaStream mediaStream)
		{
			Assert.ArgumentNotNull((object)mediaStream, nameof(mediaStream));
			Item innerItem = this.MediaData.MediaItem.InnerItem;
			if (!innerItem.Paths.IsMediaItem)
				return;
			using (new EditContext(innerItem, SecurityCheck.Disable))
			{
				innerItem["extension"] = mediaStream.Extension;
				innerItem["mime type"] = mediaStream.MimeType;
				innerItem["size"] = mediaStream.Length.ToString();
				this.EnsureAltField(innerItem);
				MediaUrlBuilderOptions shellOptions = MediaUrlBuilderOptions.GetShellOptions();
				shellOptions.Thumbnail = new bool?(true);
				shellOptions.Height = new int?(16);
				shellOptions.Width = new int?(16);
				innerItem.Appearance.Icon = MediaManager.GetMediaUrl((MediaItem)innerItem, shellOptions);
			}
		}

		/// <summary>
		/// Clones the source object. Has the same effect as creating a
		/// new instance of the type.
		/// </summary>
		/// <returns></returns>
		public override Sitecore.Resources.Media.Media Clone()
		{
			Assert.IsTrue(this.GetType() == typeof(WebPMedia), "The Clone() method must be overriden to support prototyping.");
			return (Sitecore.Resources.Media.Media)new WebPMedia();
		}

		/// <summary>Gets the stream from the pipeline 'getMediaStream'.</summary>
		/// <param name="options">The options.</param>
		/// <param name="canBeCached">The can be cached.</param>
		/// <returns>Media stream.</returns>
		protected override MediaStream GetStreamFromPipeline(
			MediaOptions options,
			out bool canBeCached)
		{
			Assert.IsNotNull((object)options, nameof(options));
			try
			{
				GetMediaStreamPipelineArgs streamPipelineArgs = new GetMediaStreamPipelineArgs(this.MediaData, options);
				CorePipeline.Run("getMediaStream", (PipelineArgs)streamPipelineArgs);
				canBeCached = true;
				return streamPipelineArgs.OutputStream;
			}
			catch (Exception ex)
			{
				Log.Error("Could not run the 'getMediaStream' pipeline for '" + this.MediaData.MediaItem.InnerItem.Paths.Path + "'. Original media data will be used.", ex, (object)this);
				canBeCached = false;
			}
			return this.MediaData.GetStream();
		}
	}
}
