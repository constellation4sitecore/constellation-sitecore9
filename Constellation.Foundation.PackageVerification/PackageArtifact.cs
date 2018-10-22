using Sitecore.Data;

namespace Constellation.Foundation.PackageVerification
{
	/// <summary>
	/// Represents a Package Artifact node in the configuration file.
	/// </summary>
	public class PackageArtifact
	{
		/// <summary>
		/// Gets or sets the ID of the Item to search for when verifying the package.
		/// </summary>
		public ID ID { get; set; }

		/// <summary>
		/// Gets or sets the name of the database to use when searching for the Item.
		/// </summary>
		public string Database { get; set; }
	}
}
