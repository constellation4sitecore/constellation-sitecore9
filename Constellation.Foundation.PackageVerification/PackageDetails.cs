using System.Collections.Generic;

namespace Constellation.Foundation.PackageVerification
{
	public class PackageDetails
	{
		public PackageDetails()
		{
			Artifacts = new List<PackageArtifact>();
		}

		public string Name { get; set; }

		public string PackageFileName { get; set; }

		public string ProcessorOverrideType { get; set; }

		public ICollection<PackageArtifact> Artifacts { get; set; }
	}
}
