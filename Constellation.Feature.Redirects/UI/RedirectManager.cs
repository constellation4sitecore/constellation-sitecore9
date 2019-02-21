using System;
using System.Web.UI.HtmlControls;
using ComponentArt.Web.UI;
using Constellation.Feature.Redirects.Models;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Security;
using Sitecore.Security.Accounts;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Text;
using Sitecore.Web.UI.Grids;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.WebControls;
using Sitecore.Web.UI.WebControls.Ribbons;
using Sitecore.Web.UI.XamlSharp.Ajax;
using Page = System.Web.UI.Page;

namespace Constellation.Feature.Redirects.UI
{
	/// <summary>
	/// Code-beside file for RedirectManager.aspx
	/// </summary>
	public partial class RedirectManager : Page, IHasCommandContext
	{
		#region Control Declarations
		/// <summary>
		/// Grid control on page.
		/// </summary>
		protected Border GridContainer;
		/// <summary>
		/// Sheer Control on page
		/// </summary>
		protected ClientTemplate LoadingFeedbackTemplate;
		/// <summary>
		/// Sheer Control on page
		/// </summary>
		protected ClientTemplate LocalNameTemplate;
		/// <summary>
		/// Sheer Control on page
		/// </summary>
		protected Ribbon Ribbon;
		/// <summary>
		/// Sheer Control on page
		/// </summary>
		protected ClientTemplate SliderTemplate;
		/// <summary>
		/// Sheer Control on page
		/// </summary>
		protected HtmlForm RedirectManagerForm;
		/// <summary>
		/// Grid control on page
		/// </summary>
		protected Grid Redirects;
		#endregion

		#region Properties
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
		#endregion

		#region Event Handlers

		/// <summary>
		/// OnInit event handler. Subscribes to Current_OnExecute
		/// </summary>
		/// <param name="e"></param>
		protected override void OnInit(EventArgs e)
		{
			Assert.ArgumentNotNull(e, "e");
			base.OnInit(e);
			Client.AjaxScriptManager.OnExecute += new AjaxScriptManager.ExecuteDelegate(Current_OnExecute);
		}

		/// <summary>
		/// OnLoad event handler. Binds the grid.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			Assert.ArgumentNotNull(e, "e");
			base.OnLoad(e);
			Assert.CanRunApplication("Redirect Manager");

			var allRedirects = Repository.GetAll();

			ComponentArtGridHandler<MarketingRedirect>.Manage(this.Redirects, new GridSource<MarketingRedirect>(allRedirects), !base.IsPostBack);
		}

		/// <summary>
		/// OnExecute event handler. Handles grid repaints.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private static void Current_OnExecute(object sender, AjaxCommandEventArgs args)
		{
			Assert.ArgumentNotNull(sender, "sender");
			Assert.ArgumentNotNull(args, "args");


			SheerResponse.Eval("refresh()");
		}
		#endregion

		CommandContext IHasCommandContext.GetCommandContext()
		{
			CommandContext context = new CommandContext();
			Item itemNotNull = Client.GetItemNotNull("/sitecore/content/Applications/Redirect Manager/Ribbon", Client.CoreDatabase);
			context.RibbonSourceUri = itemNotNull.Uri;
			string selectedValue = GridUtil.GetSelectedValue("Redirects");
			string str2 = string.Empty;
			ListString str3 = new ListString(selectedValue);
			if (str3.Count > 0)
			{
				str2 = str3[0].Split(new char[] { '^' })[0];
			}
			context.Parameters["ID"] = selectedValue;
			context.Parameters["domainname"] = SecurityUtility.GetDomainName();
			context.Parameters["accountname"] = str2;
			context.Parameters["accounttype"] = AccountType.User.ToString();
			return context;
		}
	}
}
