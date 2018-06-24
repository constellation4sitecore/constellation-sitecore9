using System;
using System.Collections.Generic;
using Constellation.Foundation.SitemapXml.Nodes;
using Sitecore.Data.Items;
using Sitecore.Web;

namespace Constellation.Foundation.SitemapXml
{
	public class NodePipeline
	{
		protected readonly ICollection<Type> ProcessorTypes;

		public NodePipeline()
		{
			ProcessorTypes = new List<Type>();


		}

		public void RunPipeline(ISitemapNode node, Item item, SiteInfo site, ICollection<ISitemapNode> output)
		{

		}
	}
}
