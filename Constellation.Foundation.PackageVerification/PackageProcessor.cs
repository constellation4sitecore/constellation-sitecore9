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
	public class PackageProcessor
	{
		protected PackageDetails Details { get; }

		public PackageProcessor(PackageDetails details)
		{
			Details = details;
		}

		public virtual void Process()
		{
			if (AllArtifactsPresent(Details.Artifacts))
			{
				Log.Info($"Constellation.Foundation.PackageVerification: package \"{Details.Name}\" is present.", this);
				return;
			}

			InstallPackage(Details.PackageFileName);
			Log.Info($"Constellation.Foundation.PackageVerification: package \"{Details.Name}\" installed.", this);
		}

		protected virtual void InstallPackage(string packageFileName)
		{
			var filePath = $"{Sitecore.Configuration.Settings.DataFolder}/packages/{packageFileName}";

			using (new SecurityDisabler())
			{
				using (new SyncOperationContext())
				{
					IProcessingContext context = new SimpleProcessingContext();
					IItemInstallerEvents events =
						new DefaultItemInstallerEvents(
							new BehaviourOptions(InstallMode.Overwrite, MergeMode.Undefined));

					context.AddAspect(events);
					IFileInstallerEvents events1 = new DefaultFileInstallerEvents(true);
					context.AddAspect(events1);

					var installer = new Sitecore.Install.Installer();
					installer.InstallPackage(MainUtil.MapPath(filePath), context);
				}
			}
		}

		protected virtual bool AllArtifactsPresent(ICollection<PackageArtifact> artifacts)
		{
			foreach (var artifact in Details.Artifacts)
			{
				if (!ArtifactVerified(artifact.Database, artifact.ID))
				{
					return false;
				}
			}

			return true;
		}


		protected virtual bool ArtifactVerified(string database, ID id)
		{
			var db = Sitecore.Configuration.Factory.GetDatabase(database);

			var item = db.GetItem(id);

			return item != null;
		}
	}
}
