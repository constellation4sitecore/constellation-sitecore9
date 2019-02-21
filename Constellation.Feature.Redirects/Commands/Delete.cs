using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
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
	public class Delete : Command, ISupportsContinuation
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
		/// Event Handler for the button press. Deletes the currently selected Marketing Redirect record.
		/// </summary>
		/// <param name="context">The Context</param>
		public override void Execute(CommandContext context)
		{
			Assert.ArgumentNotNull(context, "context");
			string str = context.Parameters["ID"];
			if (string.IsNullOrEmpty(str))
			{
				SheerResponse.Alert("Select a redirect first.", new string[0]);
			}
			else
			{
				NameValueCollection parameters = new NameValueCollection();
				parameters["ID"] = str;
				ClientPipelineArgs args = new ClientPipelineArgs(parameters);
				ContinuationManager.Current.Start(this, "Run", args);
			}
		}

		/// <summary>
		/// The actual deletion process.
		/// </summary>
		/// <param name="args">Pipeline args</param>
		protected void Run(ClientPipelineArgs args)
		{
			Assert.ArgumentNotNull(args, "args");
			ListString str = new ListString(args.Parameters["ID"]);
			if (args.IsPostBack)
			{
				if (args.Result == "yes")
				{
					List<string> list = new List<string>();

					foreach (string str2 in str)
					{
						Repository.Delete(str2);
					}

					Thread.Sleep(250); // Doing this gives Javascript time to refresh the page.
					AjaxScriptManager.Current.Dispatch("redirectmanager:redirectdeleted");
				}
			}
			else
			{
				if (str.Count == 1)
				{
					string str5 = str[0];

					SheerResponse.Confirm("Are you sure you want to delete this redirect?");
				}
				else
				{
					SheerResponse.Confirm(Translate.Text("Are you sure you want to delete these {0} redirects?", new object[] { str.Count }));
				}
				args.WaitForPostBack();
			}
		}
	}
}
