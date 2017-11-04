using System;

namespace Constellation.Foundation.SupportingItemManagement
{
	public class HasItemDependenciesAttribute
	{
		public HasItemDependenciesAttribute(string packageFileName, bool isManifest = true, Type dependencyCheck = null)
		{
			PackageFileName = packageFileName;
			IsManifest = isManifest;
			ItemCheckType = dependencyCheck;
		}

		public string PackageFileName { get; }

		public bool IsManifest { get; }

		public Type ItemCheckType { get; }
	}
}
