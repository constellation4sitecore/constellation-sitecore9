namespace Constellation.Feature.SitemapXml
{
	/// <summary>
	/// The robots txt handler configuration.
	/// </summary>
	public class RobotsTxtHandlerConfiguration
	{
		#region Locals
		private static volatile RobotsTxtHandlerConfiguration _current;

		private static object _lockObject = new object();
		#endregion

		#region Properties
		public static RobotsTxtHandlerConfiguration Current
		{
			get
			{
				if (_current == null)
				{
					lock (_lockObject)
					{
						if (_current == null)
						{
							_current = CreateNewConfiguration();
						}

					}
				}

				return _current;
			}
		}

		public bool Allowed { get; private set; }
		#endregion

		private static RobotsTxtHandlerConfiguration CreateNewConfiguration()
		{
			RobotsTxtHandlerConfiguration output = new RobotsTxtHandlerConfiguration();

			var node = Sitecore.Configuration.Factory.GetConfigNode("constellation/robotsTxt");

			if (bool.TryParse(node.Attributes["allowed"].Value, out var allowed))
			{
				output.Allowed = allowed;
			}

			return output;
		}

	}
}
