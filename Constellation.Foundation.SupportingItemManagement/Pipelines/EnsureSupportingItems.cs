using Sitecore.Pipelines;

namespace Constellation.Foundation.SupportingItemManagement.Pipelines
{
	public class EnsureSupportingItems
	{
		public void Process(PipelineArgs args)
		{
			Sitecore.Diagnostics.Assert.ArgumentNotNull(args, "args");
			// Do the things.

			if (args.Aborted)
			{
				return; // Aborted
			}

			// do things
		}
	}
}
