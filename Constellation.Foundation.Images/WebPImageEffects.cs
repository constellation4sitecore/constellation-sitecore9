using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Sitecore.Diagnostics;
using Sitecore.IO;
using Sitecore.Resources.Media;
using System.IO;

namespace Constellation.Foundation.Images
{
	public class WebPImageEffects
	{
		/// <summary>Performs required transforms on an Image loaded from the provided file path.</summary>
		/// <param name="filePath">The file path.</param>
		/// <param name="options">The transformation options to execute.</param>
		/// <returns>The file.</returns>
		public virtual Stream TransformFile(string filePath, TransformationOptions options)
		{
			Assert.ArgumentNotNullOrEmpty(filePath, nameof(filePath));
			Assert.ArgumentNotNull((object)options, nameof(options));

			filePath = FileUtil.MapPath(filePath);
			Assert.IsTrue(FileUtil.FileExists(filePath), "File to be transformed does not exist: {0}", filePath);
			using (Stream inputStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
				return this.TransformImageStream(inputStream, options);
		}

		/// <summary>Performs required transforms on an Image loaded from the provided stream.</summary>
		/// <param name="inputStream">The stream containing the media data.</param>
		/// <param name="options">The image options.</param>
		/// <returns></returns>
		public virtual Stream TransformImageStream(Stream inputStream, TransformationOptions options)
		{
			var outputStream = inputStream;

			if (!options.ContainsResizing())
			{
				Log.Debug("WebPImageEffects: No resizing parameters supplied. No transform necessary.");
				return outputStream;
			}

			if (options.Scale > 0.0)
			{
				Log.Warn("WebPImageEffects: Image transform started with non-zero Scale value. WebP resizing requires absolute pixel width or height sizing. Ignoring resize request.", this);
				return outputStream;
			}

			if (options.IgnoreAspectRatio)
			{
				Log.Info("WebPImageEffects: Image transform started with \"IgnoreAspectRatio=true\" WebP resizing cannot ignore aspect ratio. Performing resize respecting current aspect ratio.", this);
			}

			if (options.AllowStretch)
			{
				Log.Info("WebPImageEffects: Image transform started with \"AllowStretch=true\" WebP resizing cannot ignore aspect ratio. Performing resize respecting current aspect ratio.", this);
			}

			if (options.PreserveResolution)
			{
				Log.Info("WebPImageEffects: \"PreserveResolution\" was set to true. WebP resizing is by pixel, not resolution. Ignoring resolution setting.", this);
			}

			using (var factory = new ImageFactory())
			{

				if (!options.BackgroundColor.IsEmpty)
				{
					factory.BackgroundColor(options.BackgroundColor);
				}

				var webp = new WebPFormat { Quality = Sitecore.Configuration.Settings.Media.Resizing.Quality };
				factory.Load(inputStream).Resize(options.Size).Format(webp).Save(outputStream);
			}

			return outputStream;
		}

		public Stream ConvertToWebP(Stream inputStream, TransformationOptions options)
		{
			return TransformImageStream(inputStream, options);
		}

		public Stream ConvertToJpeg(Stream inputStream, TransformationOptions options)
		{
			var outputStream = TransformImageStream(inputStream, options);

			using (var factory = new ImageFactory())
			{
				var jpeg = new JpegFormat { Quality = Sitecore.Configuration.Settings.Media.Resizing.Quality };
				factory.Load(inputStream).Format(jpeg).Save(outputStream);
			}

			return outputStream;
		}

		private void PopulateTransformationOptions(TransformationOptions options)
		{
			if (options.BackgroundColor.IsEmpty)
			{
				options.BackgroundColor = Sitecore.Configuration.Settings.Media.DefaultImageBackgroundColor;
			}
		}
	}
}
