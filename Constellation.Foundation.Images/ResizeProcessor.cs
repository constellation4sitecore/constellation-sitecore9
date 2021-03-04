using Sitecore.Abstractions;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;
using Sitecore.StringExtensions;
using System;
using System.IO;

namespace Constellation.Foundation.Images
{
	/// <summary>
	/// Resizes images smaller than <see cref="P:Sitecore.Configuration.Settings.Media.MaxSizeInMemory" /> size.
	/// </summary>
	public class ResizeProcessor
	{
		/// <summary>
		/// The media manager. Provides image format via extension, and performs resizing itself.
		/// </summary>
		protected readonly BaseMediaManager MediaManager;
		/// <summary>The log to output diagnostic messages.</summary>
		protected readonly BaseLog Log;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Sitecore.Resources.Media.ResizeProcessor" /> class.
		/// </summary>
		/// <param name="mediaManager">The media manager that performs image scaling.</param>
		/// <param name="log">The log to output diagnostic messages.</param>
		public ResizeProcessor(BaseMediaManager mediaManager, BaseLog log)
		{
			this.MediaManager = mediaManager;
			this.Log = log;
		}

		/// <summary>
		/// Launches resizing for images smaller than <see cref="P:Sitecore.Configuration.Settings.Media.MaxSizeInMemory" /> size.
		/// </summary>
		/// <param name="args">The arguments.</param>
		public void Process(GetMediaStreamPipelineArgs args)
		{
			Assert.ArgumentNotNull((object)args, nameof(args));
			if (args.Options.Thumbnail)
			{
				return;
			}

			MediaStream outputStream = args.OutputStream;

			if (outputStream == null || !args.MediaData.MimeType.StartsWith("image/", StringComparison.Ordinal))
				return;
			string extension = args.MediaData.Extension;

			TransformationOptions transformationOptions = args.Options.GetTransformationOptions();
			if (!transformationOptions.ContainsResizing())
				return;
			if (!outputStream.AllowMemoryLoading)
			{
				this.Log.Error("Could not resize image as it was larger than the maximum size allowed for memory processing. Media item: {0}".FormatWith((object)outputStream.MediaItem.Path), (object)this);
			}
			else
			{
				SetTransformationBackgroundColor(transformationOptions);
				Stream transformedStream;

				if (extension == "webp")
				{
					transformedStream = new WebPImageEffects().TransformImageStream(outputStream.Stream, transformationOptions);
				}
				else
				{
					var imageFormat = MediaManager.Config.GetImageFormat(extension, null);
					if (imageFormat == null)
					{
						return;
					}
					transformedStream = MediaManager.Effects.TransformImageStream(outputStream.Stream, transformationOptions, imageFormat);
				}

				args.OutputStream = new MediaStream(transformedStream, extension, args.MediaData.MediaItem);
			}
		}

		private static void SetTransformationBackgroundColor(TransformationOptions options)
		{
			if (options.BackgroundColor.IsEmpty)
			{
				options.BackgroundColor = Settings.Media.DefaultImageBackgroundColor;
			}
		}
	}
}
