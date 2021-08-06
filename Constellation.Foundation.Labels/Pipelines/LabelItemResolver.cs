using Constellation.Foundation.Contexts.Pipelines;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;
using System;
using System.Configuration;

namespace Constellation.Foundation.Labels.Pipelines
{
	/// <summary>
	/// Allows for Label Items to be returned as JSON for consumption by AJAX clients.
	/// </summary>
	public class LabelItemResolver : ContextSensitiveHttpRequestProcessor
	{
		/// <inheritdoc />
		protected override void Execute(HttpRequestArgs args)
		{
			if (Context.Item != null)
			{
				return; // There was already a legit Item found, it's not a Label request.
			}

			var path = args.RequestUrl.LocalPath;
			var urlPrefix = GetPrefixForUrl();

			if (!IsLabelRequest(path, urlPrefix))
			{
				return;
			}

			var folderName = GetFolderForXPath();
			path = path.Replace(urlPrefix, "/" + folderName);


			// search for a Site-specific Label item.
			var itemPath = Context.Site.StartPath + path;

			var labelItem = Context.Database.GetItem(itemPath);

			if (labelItem != null)
			{
				Context.Item = labelItem;
				return;
			}

			// search for the global Label item.
			itemPath = "/sitecore/content" + path;

			Context.Item = Context.Database.GetItem(itemPath); // Null is OK.
		}

		/// <inheritdoc />
		protected override void Defer(HttpRequestArgs args)
		{
			// don't do anything.
		}

		/// <summary>
		/// Determines if the current request is requesting an API resource.
		/// </summary>
		protected bool IsLabelRequest(string path, string prefix)
		{
			return path.IndexOf(prefix, StringComparison.OrdinalIgnoreCase) == 0;
		}


		private string GetFolderForXPath()
		{
			var folderName = Sitecore.Configuration.Settings.GetSetting(SettingNames.LabelFolderName);

			if (!string.IsNullOrEmpty(folderName))
			{
				return folderName;
			}

			var message = $"Foundation.Labels - No Xml Setting found for {SettingNames.LabelFolderName} - Labels will not be available as JSON without this setting.";
			var ex = new ConfigurationErrorsException(message);

			Log.Error("Foundation.Labels: LabelItemResolver encountered an error.", ex, this);
			throw ex;

		}

		private string GetPrefixForUrl()
		{
			var urlPrefix = Sitecore.Configuration.Settings.GetSetting(SettingNames.LabelUrlPrefix);

			if (!string.IsNullOrEmpty(urlPrefix))
			{
				return urlPrefix;
			}

			var message = $"Foundation.Labels - No Xml Setting found for {SettingNames.LabelUrlPrefix} - Labels will not be available as JSON without this setting.";
			var ex = new ConfigurationErrorsException(message);

			Log.Error("Foundaiton.Labels: LabelItemResolver encountered an error.", ex, this);
			throw ex;

		}
	}
}
