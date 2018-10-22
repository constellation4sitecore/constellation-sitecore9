using System.Collections.Generic;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Engines;
using Sitecore.Diagnostics;
using Sitecore.Install.Files;
using Sitecore.Install.Framework;
using Sitecore.Install.Items;
using Sitecore.Install.Utils;
using Sitecore.SecurityModel;

namespace Constellation.Foundation.PackageVerification
{
	/// <summary>
	/// The default Processor used to verify the existence of a package's assets in Sitecore.
	/// </summary>
	public class PackageProcessor
	{
		/// <summary>
		/// Gets the configuration details to use for verification.
		/// </summary>
		protected PackageDetails Details { get; }

		/// <summary>
		/// Creates a new instance of PackageProcessor.
		/// </summary>
		/// <param name="details">The package details to verify.</param>
		public PackageProcessor(PackageDetails details)
		{
			Details = details;
		}

		/// <summary>
		/// Kicks off the verification process.
		/// </summary>
		public virtual void Process()
		{
			if (AllArtifactsPresent(Details.Artifacts))
			{
				Log.Info($"Constellation.Foundation.PackageVerification: package \"{Details.Name}\" is present.", this);
				return;
			}

			InstallPackage(Details.PackageFileName);
			Log.Info($"Constellation.Foundation.PackageVerification: package \"{Details.Name}\" installation complete.", this);
		}

		/// <summary>
		/// If verification fails, the package will be installed by this method.
		/// </summary>
		/// <param name="packageFileName">The file name of the Sitecore Package located in Sitecore's configured Packages folder.</param>
		protected virtual void InstallPackage(string packageFileName)
		{
			var filePath = $"{Sitecore.Configuration.Settings.DataFolder}/packages/{packageFileName}";

			using (new SecurityDisabler())
			{
				var installMode = Details.InstallMode;

				if (installMode == InstallMode.Undefined)
				{
					installMode = PackageVerifierConfiguration.Current.DefaultInstallMode;
				}

				var mergeMode = Details.MergeMode;
				if (mergeMode == MergeMode.Undefined)
				{
					mergeMode = PackageVerifierConfiguration.Current.DefaultMergeMode;
				}

				using (new SyncOperationContext())
				{
					IProcessingContext context = new SimpleProcessingContext();
					IItemInstallerEvents events =
						new DefaultItemInstallerEvents(
							new BehaviourOptions(installMode, mergeMode));

					context.AddAspect(events);
					IFileInstallerEvents events1 = new DefaultFileInstallerEvents(true);
					context.AddAspect(events1);

					var installer = new Sitecore.Install.Installer();
					installer.InstallPackage(MainUtil.MapPath(filePath), context);
				}
			}
		}

		/// <summary>
		/// Verifies that all artifacts specified in PackageDetails are in fact, installed.
		/// </summary>
		/// <param name="artifacts">The artifacts to verify</param>
		/// <returns>True if all Items were located.</returns>
		protected virtual bool AllArtifactsPresent(ICollection<PackageArtifact> artifacts)
		{
			foreach (var artifact in Details.Artifacts)
			{
				if (ArtifactVerified(artifact.Database, artifact.ID))
				{
					continue;
				}

				return false;
			}

			return true;
		}


		/// <summary>
		/// Verifies that a particular Item exists in a particular database.
		/// </summary>
		/// <param name="database">The database to check</param>
		/// <param name="id">The ID of the item to look for.</param>
		/// <returns>True if the database exists and the Item was found within it.</returns>
		protected virtual bool ArtifactVerified(string database, ID id)
		{
			var db = Sitecore.Configuration.Factory.GetDatabase(database);
			if (db == null)
			{
				Log.Warn($"Constellation.Foundation.PackageVerification: Database {database} does not exist in this installation.", this);
				return false;
			}

			using (new SecurityDisabler())
			{
				var item = db.GetItem(id);

				if (item != null)
				{
					Log.Debug(
						$"Constellation.Foundaiton.PackageVerification: item \"{id}\" in \"{database}\" with name {item.Name} found.",
						this);
					return true;
				}

				Log.Warn(
					$"Constellation.Foundation.PackageVerification: item \"{id}\" in \"{database}\" for package \"{Details.Name}\" was not found",
					this);
				return false;
			}
		}
	}
}
