using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Constellation.Feature.Redirects.Models;
using Sitecore.Controls;
using Sitecore.Diagnostics;
using Sitecore.Web.UI.XamlSharp.Xaml;

namespace Constellation.Feature.Redirects.UI
{
	/// <summary>
	/// Code Beside class for Export.xaml.xml
	/// </summary>
	public class Export : DialogPage
	{
		#region Control Declarations
		/// <summary>
		/// The download button
		/// </summary>
		protected Button btndownload;
		/// <summary>
		/// The success message literal
		/// </summary>
		protected Label lblSuccessMessage;
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
		/// The On Load handler. Handles event subscription
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			Assert.ArgumentNotNull(e, "e");
			base.OnLoad(e);
			if (!XamlControl.AjaxScriptManager.IsEvent)
			{
				btndownload.Click += new EventHandler(btndownload_Click);
			}
		}

		/// <summary>
		/// The Click handler for btnDownload. Handles the CSV generation and shipping.
		/// </summary>
		/// <param name="sender">the Sending object</param>
		/// <param name="e">The Event Args</param>
		protected void btndownload_Click(object sender, EventArgs e)
		{
			var allRecords = Repository.GetAll();

			if (allRecords.Any())
			{
				StringBuilder csvHeader = new StringBuilder();
				StringBuilder csv = new StringBuilder(10 * allRecords.Count * 3);

				for (int recordCount = 0; recordCount < allRecords.Count; recordCount++)
				{
					MarketingRedirect urlRedirect = allRecords[recordCount];
					StringBuilder csvRow = new StringBuilder(10 * allRecords.Count() * 3);

					for (int c = 0; c < 4; c++)
					{
						object columnValue;

						if (c != 0)
							csvRow.Append(",");

						switch (c)
						{
							case 0:
								columnValue = urlRedirect.SiteName;
								break;
							case 1:
								columnValue = urlRedirect.OldUrl;
								break;
							case 2:
								columnValue = urlRedirect.NewUrl;
								break;
							default:
								columnValue = urlRedirect.IsPermanent ? "301" : "302";
								break;
						}
						if (columnValue == null)
							csvRow.Append("");
						else
						{
							string columnStringValue = columnValue.ToString();
							string cleanedColumnValue = CleanCsvString(columnStringValue);
							csvRow.Append(cleanedColumnValue);
						}
					}
					csv.AppendLine(csvRow.ToString());
				}

				HttpContext context = HttpContext.Current;
				context.Response.Clear();

				context.Response.Write(csvHeader);
				context.Response.Write(csv.ToString());
				context.Response.Write(Environment.NewLine);

				context.Response.ContentType = "text/csv";
				context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + "RedirectUrls" + ".csv");
				context.Response.End();
			}
			else
			{
				this.btndownload.Visible = false;
				lblSuccessMessage.Text = "No Records found to Export Redirects.";
				lblSuccessMessage.Visible = true;
			}

		}
		#endregion

		/// <summary>
		/// Formats a given row of the CSV to ensure integrity of the document.
		/// </summary>
		/// <param name="input">The string to format.</param>
		/// <returns>A formatted string ready for export.</returns>
		protected string CleanCsvString(string input)
		{
			string output = "\"" + input.Replace("\"", "\"\"").Replace("\r\n", " ").Replace("\r", " ").Replace("\n", "") + "\"";
			return output;
		}
	}
}

