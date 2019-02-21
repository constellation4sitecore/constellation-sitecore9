using System;
using System.Threading;
using Constellation.Feature.Redirects.Models;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Text;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.WebControls;
using Sitecore.Web.UI.XamlSharp.Continuations;
using Convert = System.Convert;

namespace Constellation.Feature.Redirects.Commands
{
	/// <summary>
	/// Command associated with the Delete button in the Redirect Manager's ribbon.
	/// </summary>
	[Serializable]
	public class New : Command, ISupportsContinuation
	{
		/// <summary>
		/// Event handler for the button press that opens the new record dialog.
		/// </summary>
		/// <param name="context"></param>
		public override void Execute(CommandContext context)
		{
			Assert.ArgumentNotNull(context, "context");
			ContinuationManager.Current.Start(this, "Run");
		}

		/// <summary>
		/// Handles the recording of the new record into the Sitecore database.
		/// </summary>
		/// <param name="args">The pipeline args</param>
		protected static void Run(ClientPipelineArgs args)
		{
			Assert.ArgumentNotNull(args, "args");

			if (!args.IsPostBack)
			{
				ShowModal(args);
				return;
			}

			if (!args.HasResult)
			{
				return;
			}

			string results = args.Result;
			AjaxScriptManager ajaxScriptManager = Client.AjaxScriptManager;

			if (string.IsNullOrEmpty(results))
			{
				ajaxScriptManager.Alert("Enter an old URL and a new URL for the new redirect.");
				return;
			}

			string[] values = results.Split('|');


			var candidate = new MarketingRedirect
			{
				IsPermanent = MainUtil.GetBool(Convert.ToInt32(values[0]), false),
				OldUrl = values[1].ToLower().Trim(),
				NewUrl = values[2].ToLower().Trim(),
				SiteName = values[3]
			};

			var repository = new Repository(Sitecore.Context.ContentDatabase, "sitecore_master_index");

			if (repository.CandidateOldUrlContainsHostname(candidate))
			{
				SheerResponse.Alert(Translate.Text("Only partial URLs are allowed in the Old URL field."));
				ShowModal(args);
				return;
			}

			if (!repository.CandidateIsUnique(candidate))
			{
				SheerResponse.Alert(
					Translate.Text("A redirect with the old URL \"{0}\" already exists.", new object[] { candidate.OldUrl }), new string[0]);
				ShowModal(args);
				return;
			}

			if (repository.CandidateTargetIsRedirect(candidate))
			{
				SheerResponse.Alert("This redirect is invalid because the new URL provided would also be redirected due to another rule in this system. The New URL must resolve to a published page.");
				ShowModal(args);
				return;
			}

			if (!repository.CandidateTargetReturnsHttpSuccessResponse(candidate, out var status))
			{
				SheerResponse.Alert(status.Message);
				ShowModal(args);
				return;
			}

			try
			{
				repository.Insert(candidate.SiteName, candidate.OldUrl, candidate.NewUrl, candidate.IsPermanent);

				Thread.Sleep(250); // If we don't do this, the refresh call won't work.
				ajaxScriptManager.Dispatch("redirectmanager:refresh");
			}
			catch (Exception exception)
			{
				ajaxScriptManager.Alert(Translate.Text("An error occurred while creating the redirect for\"\":\n\n{1}", new object[] { values[1], exception.Message }));
				ShowModal(args);
			}
		}

		private static void ShowModal(ClientPipelineArgs args)
		{
			var sheerUrl = new UrlString("/sitecore/shell/~/xaml/Sitecore.SitecoreModule.Shell.Redirect.NewRedirect.aspx");
			SheerResponse.ShowModalDialog(sheerUrl.ToString(), "780", "350", string.Empty, true);
			args.WaitForPostBack();
		}
	}
}