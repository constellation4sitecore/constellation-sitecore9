using System;
using System.Web.UI.WebControls;
using Sitecore.Controls;
using Sitecore.Diagnostics;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.XamlSharp.Xaml;

namespace Constellation.Feature.Redirects.UI
{
	/// <summary>
	/// Code Beside class for NewRedirect.xaml.xml
	/// </summary>
	public class NewRedirect : DialogPage
	{
		#region Control Declarations
		/// <summary>
		/// The dropDown control for IsPermanent
		/// </summary>
		protected DropDownList Type;
		/// <summary>
		/// The textbox for Old Url
		/// </summary>
		protected TextBox OldUrl;
		/// <summary>
		/// The textbox for New Url
		/// </summary>
		protected TextBox NewUrl;
		/// <summary>
		/// The dropDown control for Site Name
		/// </summary>
		protected DropDownList SiteName;
		#endregion


		#region Event Handlers
		/// <summary>
		/// OK Click handler
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
				SheerResponse.Alert("The Old URL, the New URL and the Site name cannot be empty.");
			}
			else
			{
				SheerResponse.SetDialogValue($"{this.Type.SelectedValue}|{oldUrl}|{newUrl}|{siteName}");
				base.OK_Click();
			}
		}

		/// <summary>
		/// OnLoad Handler. Binds controls.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			Assert.ArgumentNotNull(e, "e");
			base.OnLoad(e);
			if (!XamlControl.AjaxScriptManager.IsEvent)
			{
				PopulateTypeDropDown();
				PopulateSiteNameDropDown();
			}
		}
		#endregion

		private void PopulateTypeDropDown()
		{
			ListItem itm301 = new ListItem("301", "1");
			ListItem itm302 = new ListItem("302", "0");
			this.Type.Items.Add(itm301);
			this.Type.Items.Add(itm302);
			itm301.Selected = true;
		}

		private void PopulateSiteNameDropDown()
		{
			var sites = Sitecore.Configuration.Factory.GetSiteInfoList();
			const string systemSiteNames = "shell,login,admin,service,modules_shell,modules_website,scheduler,system,publisher";

			foreach (var site in sites)
			{
				if (systemSiteNames.Contains(site.Name))
				{
					continue;
				}

				var option = new ListItem($"{site.Name} ({site.Scheme}://{site.TargetHostName}{site.VirtualFolder})", site.Name);

				this.SiteName.Items.Add(option);
			}

			if (this.SiteName.Items.Count > 0)
			{
				this.SiteName.Items[0].Selected = true;
			}
		}
	}
}
