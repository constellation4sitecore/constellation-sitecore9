using System;
using System.Xml;

namespace Constellation.Foundation.Caching
{
	/// <summary>
	/// Clears the caching of the custom site caching objects
	/// </summary>
	public class CacheClearingAgent
	{
		#region OnPublishEnd
		/// <summary>
		/// On Publish End event to clear the caching from the framework
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void OnPublishEnd(object sender, EventArgs args)
		{
			//item saved is called when an item is published (because the item is being saved in the web database)
			//when this happens, we don't want our code to move the item anywhere, so escape out of this function.
			if (Sitecore.Context.Job != null && !Sitecore.Context.Job.Category.Equals("publish", StringComparison.OrdinalIgnoreCase))
			{
				return;
			}

			string eventName;

			if (!args.GetType().ToString().Equals("Sitecore.Data.Events.PublishEndRemoteEventArgs"))
			{
				eventName = ((Sitecore.Events.SitecoreEventArgs)args).EventName;
			}
			else
			{
				eventName = "publish:end:remote";
			}

			// get the site list
			var siteList = Sitecore.Configuration.Factory.GetConfigNodes($"/sitecore/events/event[@name='{eventName}']/handler[@type='Sitecore.Publishing.HtmlCacheClearer, Sitecore.Kernel']/sites/site");

			// make sure we have a site list to clean up
			if (siteList != null)
			{
				foreach (XmlNode xNode in siteList)
				{
					var site = Sitecore.Configuration.Factory.GetSite(xNode.InnerText);
					if (site != null)
					{
						SitecoreCacheManager.ClearCache(site.Name, site.Database.Name);
					}
				}
			}
		}
		#endregion
	}

}