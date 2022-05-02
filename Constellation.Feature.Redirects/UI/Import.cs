using Constellation.Feature.Redirects.Models;
using Sitecore.Controls;
using Sitecore.Diagnostics;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace Constellation.Feature.Redirects.UI
{
	/// <summary>
	/// Code Beside class for Import.xaml.xml
	/// </summary>
	public class Import : DialogPage
	{
		#region Control Declarations
		/// <summary>
		/// The Import File selector
		/// </summary>
		protected FileUpload fileImport;
		/// <summary>
		/// the upload button
		/// </summary>
		protected Button Upload;
		/// <summary>
		/// The status message label
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
		/// OnLoad Event Handler. Subscribes to button events.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{

			Assert.ArgumentNotNull(e, "e");
			base.OnLoad(e);
			Upload.Click += new EventHandler(Upload_Click);
		}

		/// <summary>
		/// Click event handler for Upload button.
		/// </summary>
		/// <param name="sender">the sender</param>
		/// <param name="e">the event args</param>
		protected void Upload_Click(object sender, EventArgs e)
		{
			string filename = fileImport.FileName;
			string[] fileExt = filename.Split('.');
			string fileEx = fileExt[fileExt.Length - 1];

			int noSite = 0;
			int wrongFormat = 0;
			int duplicates = 0;
			int redirectLoops = 0;
			int targetHttpErrors = 0;
			int newRecords = 0;
			int totalRecords = 0;

			if (fileEx.ToLower() == "csv")
			{
				Stream theStream = fileImport.PostedFile.InputStream;
				using (StreamReader sr = new StreamReader(theStream))
				{
					string line;
					while ((line = sr.ReadLine()) != null)
					{
						totalRecords++;

						var regex = new Regex("(?<=^|,)(\"(?:[^\"]|\"\")*\"|[^,]*)");
						var matches = regex.Matches(line);

						var candidate = new MarketingRedirect();

						candidate.SiteName = matches[0].Value.Replace("\"", "");
						candidate.OldUrl = matches[1].Value.Replace("\"", "");
						candidate.NewUrl = matches[2].Value.Replace("\t", "").Replace("\"", "");
						candidate.IsPermanent = matches[3].Value.Replace("\t", "").Replace("\"", "").Equals("301");

						if (!Repository.CandidateHasValidSiteName(candidate))
						{
							noSite++;
							continue;
						}

						if (Repository.CandidateOldUrlContainsHostname(candidate))
						{
							wrongFormat++;
							continue;
						}

						if (!Repository.CandidateIsUnique(candidate))
						{
							duplicates++;
							continue;
						}

						if (Repository.CandidateTargetIsRedirect(candidate))
						{
							redirectLoops++;
							continue;
						}

						if (!Repository.CandidateTargetReturnsHttpSuccessResponse(candidate, out var status))
						{
							targetHttpErrors++;
							continue;
						}

						Repository.Insert(candidate);
						newRecords++;
					}
				}

				var message = new StringBuilder($"Successfully parsed {totalRecords} records.");

				if (newRecords > 0)
				{
					message.Append($"<br>{newRecords} new records added.");
				}

				if (noSite > 0)
				{
					message.Append($"<br>{noSite} records with invalid site names skipped.");
				}

				if (wrongFormat > 0)
				{
					message.Append($"<br>{wrongFormat} hostnames in OldURL column skipped.");
				}

				if (redirectLoops > 0)
				{
					message.Append($"<br>{redirectLoops} redirect loops skipped.");
				}

				if (targetHttpErrors > 0)
				{
					message.Append($"<br>{targetHttpErrors} records where destination URLs did not return HTTP Success were skipped.");
				}

				if (duplicates > 0)
				{
					message.Append($"<br>{duplicates} duplicates ignored.");
				}

				if (wrongFormat > 0 || redirectLoops > 0 || redirectLoops > 0 || targetHttpErrors > 0 || duplicates > 0)
				{
					message.Append("<br>To troubleshoot, export the current list and compare it against your import.");
				}

				fileImport.Visible = false;
				Upload.Visible = false;
				lblSuccessMessage.Text = message.ToString();
				lblSuccessMessage.Visible = true;
			}
			else
			{
				fileImport.Visible = false;
				this.Upload.Visible = false;
				lblSuccessMessage.Text = "File name must end in \".csv\" and must be a comma-delimited data file.";
				lblSuccessMessage.Visible = true;
			}
		}
		#endregion
	}
}