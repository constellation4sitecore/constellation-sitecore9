using Sitecore.Resources.Media;
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

		public override void UpdateMetaData(MediaStream mediaStream)
		{
			base.UpdateMetaData(mediaStream);
		}

		public override void SetImage(Image image)
		{
			base.SetImage(image);
		}

		public override Image GetImage()
		{
			return base.GetImage();
		}


	}
}
