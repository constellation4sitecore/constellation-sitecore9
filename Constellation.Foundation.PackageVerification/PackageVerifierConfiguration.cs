using System;
using System.Collections.Generic;
using System.Xml;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Install.Utils;

namespace Constellation.Foundation.PackageVerification
{
	/// <summary>
	/// Represents the configuration file settings for PackageVerifier.
	/// </summary>
	public class PackageVerifierConfiguration
	{
		#region Locals

		private static volatile PackageVerifierConfiguration _current;

		private static readonly object LockObject = new object();

		#endregion

		#region Properties

		/// <summary>
		/// Gets (or creates) the current instance of Package Verifier's configuration.
		/// </summary>
		public static PackageVerifierConfiguration Current
		{
			get
			{
				if (_current == null)
				{
					lock (LockObject)
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

		/// <summary>
		/// Gets the default package processor type.
		/// </summary>
		public Type DefaultProcessorType { get; private set; }

		/// <summary>
		/// Gets the default install mode for packages.
		/// </summary>
		public InstallMode DefaultInstallMode { get; private set; }

		/// <summary>
		/// Gets the default merge mode for packages.
		/// </summary>
		public MergeMode DefaultMergeMode { get; private set; }

		/// <summary>
		/// Gets details for all of the packages that should be verified as installed.
		/// </summary>
		public ICollection<PackageDetails> Packages { get; private set; }

		#endregion

		/// <summary>
		/// Creates a new instance of PackageVerifierConfiguration.
		/// </summary>
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

			output.DefaultInstallMode = GetInstallMode(verifierNode.Attributes?["defaultInstallMode"]?.Value);
			output.DefaultMergeMode = GetMergeMode(verifierNode.Attributes?["defaultMergeMode"]?.Value);

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
				package.InstallMode = GetInstallMode(packageNode?.Attributes?["installMode"]?.Value);
				package.MergeMode = GetMergeMode(packageNode?.Attributes?["mergeMode"]?.Value);


				if (!packageNode.HasChildNodes)
				{
					Log.Warn($"Constellation.Foundation.PackageVerification: Package configuration named \"{package.Name}\" has no mandatory items listed.", output);
					continue;
				}

				var artifactNodes = packageNode.ChildNodes;

				/*
					<packageVerifier defaultProcessorType="Constellation.Foundation.PackageVerification.PackageProcessor, Constellation.Foundation.PackageVerification">
						<package name="example" packageFileName="example-9.0.6.0.zip">
							<artifact id="{AE9999EE-92F0-4E91-9123-1093CCFBBEBD}" database="core"/>
							<artifact id="{80B44127-950B-4023-9EDE-D74CE2760242}" database="core"/>
							<artifact id="{DCF35CDA-E30C-4BE7-AE43-E56BE8AF07E5}" database="master" />
							<artifact id="{3D2EC9DC-52EA-4355-97D1-34BBAD390E89}" database="master"/>
						</package>
					</packageVerifier>
				 */



				foreach (XmlNode artifactNode in artifactNodes)
				{
					if (artifactNode == null)
					{
						continue;
					}

					string id = artifactNode.Attributes?["id"]?.Value;

					string database = artifactNode.Attributes?["database"]?.Value;

					if (string.IsNullOrEmpty(id))
					{
						var ex = new Exception(
							$"Failed parsing artifact id attribute for package \"{package.Name}\".");

						Log.Error("Constellation.Foundation.PackageVerification: Error in loading Configuration.", ex, output);
						throw ex;
					}

					if (string.IsNullOrEmpty(database))
					{
						var ex = new Exception(
							$"Failed parsing artifact database attribute for package \"{package.Name}\".");

						Log.Error("Constellation.Foundation.PackageVerification: Error in loading Configuration.", ex, output);
						throw ex;
					}

					var artifact = new PackageArtifact
					{
						ID = ID.Parse(id),
						Database = database
					};

					package.Artifacts.Add(artifact);
				}
			}

			return output;
		}

		private static InstallMode GetInstallMode(string mode)
		{
			if (string.IsNullOrEmpty(mode))
			{
				return InstallMode.Undefined;
			}

			var installMode = mode.ToLower();

			switch (installMode)
			{
				case "merge":
					return InstallMode.Merge;
				case "overwrite":
					return InstallMode.Overwrite;
				case "sidebyside":
					return InstallMode.SideBySide;
				case "skip":
					return InstallMode.Skip;
				default:
					return InstallMode.Undefined;
			}
		}

		private static MergeMode GetMergeMode(string mode)
		{
			if (string.IsNullOrEmpty(mode))
			{
				return MergeMode.Undefined;
			}

			var megeMode = mode.ToLower();

			switch (megeMode)
			{
				case "append":
					return MergeMode.Append;
				case "clear":
					return MergeMode.Clear;
				case "merge":
					return MergeMode.Merge;
				default:
					return MergeMode.Undefined;
			}
		}
	}
}
