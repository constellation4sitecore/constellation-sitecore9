using System;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;

namespace Constellation.Foundation.PackageVerification.Pipelines.Initialize
{
	/// <summary>
	/// Creates a new instance of PackageVerifier
	/// </summary>
	public class PackageVerifier
	{
		/// <summary>
		/// Called by the Sitecore Initialize Pipeline.
		/// </summary>
		/// <param name="args">The pipeline arguments.</param>
		public void Process(PipelineArgs args)
		{
			var packages = PackageVerifierConfiguration.Current.Packages;

			Log.Info($"Constellation.Foundation.PackageVerification found {packages.Count} packages to verify.", this);

			foreach (var config in packages)
			{
				Log.Info($"Constellation.Foundation.PackageVerification: processing \"{config.Name}\" package.", this);

				try
				{
					var processorType = PackageVerifierConfiguration.Current.DefaultProcessorType;
					if (!string.IsNullOrEmpty(config.ProcessorOverrideType))
					{
						processorType = Type.GetType(config.ProcessorOverrideType, true);
					}

					var processor = (PackageProcessor)Activator.CreateInstance(processorType, config);

					processor.Process();
				}
				catch (Exception ex)
				{
					Log.Error($"Constellation.Foundation.PackageVerification failed processing \"{config.Name}\" package.", ex, this);
					throw;
				}
			}
		}
	}
}
