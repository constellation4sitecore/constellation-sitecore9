using System;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Text;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.WebControls;
using Sitecore.Web.UI.XamlSharp.Continuations;

namespace Constellation.Feature.Redirects.Commands
{
	/// <summary>
	/// Command associated with the Delete button in the Redirect Manager's ribbon.
	/// </summary>
	[Serializable]
	public class Import : Command, ISupportsContinuation
	{
		/// <summary>
		/// The button press event handler
		/// </summary>
		/// <param name="context">The command context</param>
		public override void Execute(CommandContext context)
		{
			Assert.ArgumentNotNull(context, "context");
			ContinuationManager.Current.Start(this, "Run");
		}

		/// <summary>
		/// Dispatches execution to the Import dialog.
		/// </summary>
		/// <param name="args">The pipeline args.</param>
		protected static void Run(ClientPipelineArgs args)
		{
			Assert.ArgumentNotNull(args, "args");
			if (args.IsPostBack)
			{
				AjaxScriptManager.Current.Dispatch("redirectmanager:refresh");
			}
			else
			{
				UrlString str2 = new UrlString("/sitecore/shell/~/xaml/Sitecore.SitecoreModule.Shell.Redirect.Import.aspx");
				SheerResponse.ShowModalDialog(str2.ToString(), "450", "250", string.Empty, true);
				args.WaitForPostBack();
			}
		}
	}
}
