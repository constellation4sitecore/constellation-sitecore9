using System;
using System.Collections.Specialized;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.WebControls;
using Sitecore.Web.UI.XamlSharp.Continuations;

namespace Constellation.Feature.Redirects.Commands
{
	/// <summary>
	/// Command associated with the Clear All button in the Redirect Manager's Ribbon.
	/// </summary>
	[Serializable]
	public class ClearAll : Command, ISupportsContinuation
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
		/// Event Handler for the button press. Deletes all Items in the Marketing Redirects bucket.
		/// </summary>
		/// <param name="context">The Context</param>
		public override void Execute(CommandContext context)
		{
			Assert.ArgumentNotNull(context, "context");

			NameValueCollection parameters = new NameValueCollection();
			ClientPipelineArgs args = new ClientPipelineArgs(parameters);
			ContinuationManager.Current.Start(this, "Run", args);
		}

		/// <summary>
		/// The actual deletion process.
		/// </summary>
		/// <param name="args">Pipeline args.</param>
		protected void Run(ClientPipelineArgs args)
		{
			Assert.ArgumentNotNull(args, "args");

			if (args.IsPostBack)
			{
				if (args.Result == "yes")
				{
					Repository.DeleteAll();
					AjaxScriptManager.Current.Dispatch("redirectmanager:redirectdeleted");
				}
			}
			else
			{
				SheerResponse.Confirm(
					"WARNING: This will delete all marketing redirects in the database. Do you want to continue?");

				args.WaitForPostBack();
			}
		}
	}
}
