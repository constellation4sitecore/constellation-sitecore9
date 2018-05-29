using System;
using System.Collections.Generic;
using System.Xml;
using Sitecore.Data;
using Sitecore.Diagnostics;

namespace Constellation.Foundation.PackageVerification
{
	public class PackageVerifierConfiguration
	{
		#region Locals

		private static volatile PackageVerifierConfiguration _current;

		private static object _lockObject = new object();

		#endregion

		#region Properties

		public static PackageVerifierConfiguration Current
		{
			get
			{
				if (_current == null)
				{
					lock (_lockObject)
					{
						if (_current == null)
						{
							_current = CreateNewConfiguration();
						}
					}
				}

				return _current;
			}
		}

		public Type DefaultProcessorType { get; private set; }

		public ICollection<PackageDetails> Packages { get; private set; }

		#endregion

		protected PackageVerifierConfiguration()
		{
			Packages = new List<PackageDetails>();
		}


		private static PackageVerifierConfiguration CreateNewConfiguration()
		{
			var output = new PackageVerifierConfiguration();

			var verifierNode = Sitecore.Configuration.Factory.GetConfigNode("constellation/packageVerifier");

			if (verifierNode == null)
			{
				var ex = new Exception("Constellation.Foundation.PackageVerification: configuration requires a /sitecore/constellation/packageVerifier node. Not found!");
				Log.Error("Constellation.Foundation.PackageVerification cannot continue.", ex, output);
				throw ex;
			}

			var defaultType = verifierNode.Attributes?["defaultProcessorType"]?.Value;

			if (!string.IsNullOrEmpty(defaultType))
			{
				output.DefaultProcessorType = Type.GetType(defaultType, true);
			}
			else
			{
				output.DefaultProcessorType = typeof(PackageProcessor);
			}

			if (!verifierNode.HasChildNodes)
			{
				Log.Warn("Constellation.Foundation.PackageVerification didn't find any package configurations.", output);
				return output;
			}

			var packageNodes = verifierNode.ChildNodes;
			foreach (XmlNode packageNode in packageNodes)
			{
				if (packageNode == null)
				{
					continue;
				}



				var package = new PackageDetails();

				output.Packages.Add(package);

				package.Name = packageNode?.Attributes?["name"]?.Value;
				package.PackageFileName = packageNode?.Attributes?["packageFileName"]?.Value;
				package.ProcessorOverrideType = packageNode?.Attributes?["processorOverrideType"]?.Value;

				if (!packageNode.HasChildNodes)
				{
					Log.Warn($"Constellation.Foundation.PackageVerification: Package configuration named \"{package.Name}\" has no mandatory items listed.", output);
					continue;
				}

				var artifactNodes = packageNode.ChildNodes;

				foreach (XmlNode artifactNode in artifactNodes)
				{
					if (artifactNode == null)
					{
						continue;
					}

					var artifact = new PackageArtifact
					{
						ID = ID.Parse(artifactNode?.Attributes?["id"]?.Value),
						Database = artifactNode?.Attributes?["database"]?.Value
					};

					package.Artifacts.Add(artifact);
				}
			}

			return output;
		}
	}
}
