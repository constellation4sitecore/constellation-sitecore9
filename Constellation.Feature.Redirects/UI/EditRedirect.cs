using System;
using System.Linq;
using System.Web.UI.WebControls;
using Constellation.Feature.Redirects.Models;
using Sitecore.Controls;
using Sitecore.Diagnostics;
using Sitecore.Web;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.XamlSharp.Xaml;

namespace Constellation.Feature.Redirects.UI
{
	/// <summary>
	/// Code Beside class for EditRedirect.xaml.xml
	/// </summary>
	public class EditRedirect : DialogPage
	{
		#region Controls
		/// <summary>
		/// The IsPermanent drop down list control
		/// </summary>
		protected DropDownList Type;
		/// <summary>
		/// The Old Url textbox control
		/// </summary>
		protected TextBox OldUrl;
		/// <summary>
		/// The New Url textbox control
		/// </summary>
		protected TextBox NewUrl;
		/// <summary>
		/// The Site Name drop down list control
		/// </summary>
		protected DropDownList SiteName;
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
		/// The OK Click handler
		/// </summary>
		protected override void OK_Click()
		{
			string oldUrl = this.OldUrl.Text.Trim();
			string newUrl = this.NewUrl.Text.Trim();
			string siteName = "website";

			if (this.SiteName.SelectedIndex > -1)
			{
				siteName = this.SiteName.SelectedValue;
			}

			if (string.IsNullOrEmpty(oldUrl) || string.IsNullOrEmpty(newUrl) || string.IsNullOrEmpty(siteName))
			{
				SheerResponse.Alert("The Old URL, New URL and Site name cannot be empty.");
			}
			else
			{
				SheerResponse.SetDialogValue($"{this.Type.SelectedValue}|{oldUrl}|{newUrl}|{siteName}");
				base.OK_Click();
			}
		}

		/// <summary>
		/// The OnLoad handler. Binds controls
		/// </summary>
		/// <param name="e">The EventArgs</param>
		protected override void OnLoad(EventArgs e)
		{
			Assert.ArgumentNotNull(e, "e");
			base.OnLoad(e);
			if (!XamlControl.AjaxScriptManager.IsEvent)
			{
				string id = WebUtil.GetQueryString("ID");
				if (!string.IsNullOrEmpty(id))
				{
					var record = Repository.GetById(id);

					if (record != null)
					{
						OldUrl.Text = record.OldUrl;
						NewUrl.Text = record.NewUrl;
						PopulateTypeDropDownList(record);
						PopulateSiteNameDropDownList(record);
					}
				}
			}
		}
		#endregion

		private void PopulateTypeDropDownList(MarketingRedirect record)
		{
			ListItem itm301 = new ListItem("Permanent, 301", "1");
			ListItem itm302 = new ListItem("Temporary, 302", "0");
			itm301.Selected = record.IsPermanent;
			itm302.Selected = !record.IsPermanent;

			this.Type.Items.Add(itm301);
			this.Type.Items.Add(itm302);
		}

		private void PopulateSiteNameDropDownList(MarketingRedirect record)
		{
			var sites = Sitecore.Configuration.Factory.GetSiteInfoList().Distinct();
			const string systemSiteNames = ",shell,login,admin,service,modules_shell,modules_website,scheduler,system,publisher,";

			foreach (var site in sites)
			{
				// The commas here ensure that the complete name is evaluated, otherwise "website" ends up being a partial match.
				if (systemSiteNames.Contains($",{site.Name},"))
				{
					continue;
				}

				if (string.IsNullOrEmpty(site.TargetHostName))
				{
					continue;
				}

				var option = new ListItem($"{site.Name} ({site.Scheme}://{site.TargetHostName}{site.VirtualFolder})", site.Name);

				if (option.Value == record.SiteName)
				{
					option.Selected = true;
				}

				this.SiteName.Items.Add(option);
			}
		}
	}
}