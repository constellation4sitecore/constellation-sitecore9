using Constellation.Feature.Redirects.Models;
using Sitecore;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Text;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.WebControls;
using Sitecore.Web.UI.XamlSharp.Continuations;
using System;
using System.Collections.Specialized;
using System.Threading;
using Convert = System.Convert;

namespace Constellation.Feature.Redirects.Commands
{
	/// <summary>
	/// Command associated with the Delete button in the Redirect Manager's ribbon.
	/// </summary>
	[Serializable]
	public class Edit : Command, ISupportsContinuation
	{
		private Repository _repository;
		/// <summary>
		/// The Redirect Repository
		/// </summary>
		protected Repository Repository
		{
			get
			{
				if (_repository == null)
				{
					_repository = new Repository(Sitecore.Context.ContentDatabase, "sitecore_master_index");
				}

				return _repository;
			}
		}

		/// <summary>
		/// Event Handler for the button press. Opens the currently selected Marketing Redirect for editing.
		/// </summary>
		/// <param name="context">The Context</param>
		public override void Execute(CommandContext context)
		{
			Assert.ArgumentNotNull(context, "context");
			string strID = context.Parameters["ID"];
			if (string.IsNullOrEmpty(strID))
			{
				SheerResponse.Alert("Select a redirect first.", new string[0]);
			}
			else
			{
				NameValueCollection parameters = new NameValueCollection();
				parameters["ID"] = strID;
				ClientPipelineArgs args = new ClientPipelineArgs(parameters);
				ContinuationManager.Current.Start(this, "Run", args);
			}
		}

		/// <summary>
		/// Handles the process of committing record changes to the Sitecore database.
		/// </summary>
		/// <param name="args">the pipeline args.</param>
		protected void Run(ClientPipelineArgs args)
		{
			Assert.ArgumentNotNull(args, "args");
			AjaxScriptManager ajaxScriptManager = Client.AjaxScriptManager;

			if (args.IsPostBack)
			{

				if (!args.HasResult)
				{
					return;
				}
				string results = args.Result;
				if (string.IsNullOrEmpty(results))
				{
					ajaxScriptManager.Alert("Enter an old URL and a new URL for the new redirect.");
					return;
				}

				string[] values = results.Split('|');


				var changes = new MarketingRedirect();

				changes.ItemId = new ID(args.Parameters["ID"]);
				changes.IsPermanent = MainUtil.GetBool(Convert.ToInt32(values[0]), false);
				changes.OldUrl = values[1].ToLower().Trim();
				changes.NewUrl = values[2].Trim();
				changes.SiteName = values[3];


				if (Repository.CandidateOldUrlContainsHostname(changes))
				{
					SheerResponse.Alert(Translate.Text("Only partial URLs are allowed in the Old URL field."));
					return;
				}

				if (!Repository.CandidateIsUnique(changes))
				{
					SheerResponse.Alert(
						Translate.Text("A redirect with the old URL \"{0}\" already exists.", new object[] { values[1] }),
						new string[0]);
					return;
				}

				if (Repository.CandidateTargetIsRedirect(changes))
				{
					SheerResponse.Alert("The destination URL of this redirect is also a redirect. New URL must point to a page.");
					return;
				}

				if (!Repository.CandidateTargetReturnsHttpSuccessResponse(changes, out var status))
				{
					SheerResponse.Alert(status.Message);
					return;
				}


				try
				{
					Repository.Update(changes);

					Thread.Sleep(250); // If we don't do this, the refresh call might not work.
					ajaxScriptManager.Dispatch("redirectmanager:refresh");
				}
				catch (Exception exception)
				{
					ajaxScriptManager.Alert(
						Translate.Text("An error occured while creating the redirect for\"\":\n\n{1}",
							new object[] { values[1], exception.Message }));
				}
			}
			else
			{
				ajaxScriptManager.Alert("Are you sure you want to Edit this redirect?");
				UrlString editRedirectUrl = new UrlString("/sitecore/shell/~/xaml/Sitecore.SitecoreModule.Shell.Redirect.EditRedirect.aspx");
				editRedirectUrl.Parameters["ID"] = args.Parameters["ID"];
				SheerResponse.ShowModalDialog(editRedirectUrl.ToString(), "780", "350", string.Empty, true);
				args.WaitForPostBack();
			}
		}
	}
}