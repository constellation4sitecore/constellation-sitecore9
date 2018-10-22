using System.Collections.Generic;
using Sitecore.Install.Utils;

namespace Constellation.Foundation.PackageVerification
{
	/// <summary>
	/// The configuration the Package Verifier should use when inspecting Sitecore for a Package.
	/// </summary>
	public class PackageDetails
	{
		/// <summary>
		/// Creates a new instance of Package Details.
		/// </summary>
		public PackageDetails()
		{
			Artifacts = new List<PackageArtifact>();
		}

		/// <summary>
		/// Gets or sets the human-legible name for the package. Used in Log files.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the name of the Package file as it appears in Sitecore's configured Packages folder.
		/// </summary>
		public string PackageFileName { get; set; }

		/// <summary>
		/// Gets or sets the type Full Name for the package processor that should be used to verify package installation for the named package.
		/// </summary>
		public string ProcessorOverrideType { get; set; }

		/// <summary>
		/// Gets or sets the Package Installer install mode (how it handles duplicate items).
		/// </summary>
		public InstallMode InstallMode { get; set; }

		/// <summary>
		/// Gets or sets the Packager Installer merge mode (how it handles duplicate branches).
		/// </summary>
		public MergeMode MergeMode { get; set; }

		/// <summary>
		/// Gets or sets the collection of Artifacts that must exist for the package to pass verification.
		/// </summary>
		public ICollection<PackageArtifact> Artifacts { get; set; }
	}
}
