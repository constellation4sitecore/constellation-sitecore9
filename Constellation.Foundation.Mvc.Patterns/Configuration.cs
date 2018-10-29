using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using Sitecore.Diagnostics;

namespace Constellation.Foundation.Mvc.Patterns
{
	/// <summary>
	/// The Runtime Settings for Constellation Mvc Patterns.
	/// </summary>
	public class Configuration
	{
		#region Locals
		private static volatile Configuration _current;

		private static readonly object LockObject = new object();
		#endregion

		#region Constructor

		/// <summary>
		/// Creates a new instance of Configuration
		/// </summary>
		protected Configuration()
		{
			AssembliesToRegister = new List<Assembly>();
		}
		#endregion


		#region Properties

		/// <summary>
		/// The current runtime settings for Constellation Mvc Patterns
		/// </summary>
		public static Configuration Current
		{
			get
			{
				if (_current == null)
				{
					lock (LockObject)
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

		/// <summary>
		/// A list of the Assemblies to parse for Repositories and Controllers
		/// </summary>
		public ICollection<Assembly> AssembliesToRegister { get; private set; }
		#endregion

		#region Methods

		private static Configuration CreateNewConfiguration()
		{
			var output = new Configuration();

			var libraryNode = Sitecore.Configuration.Factory.GetConfigNode("constellation/mvc.patterns/registerServices");

			if (libraryNode == null || !libraryNode.HasChildNodes)
			{
				return output;
			}

			var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();


			var adds = libraryNode.ChildNodes;

			foreach (XmlNode addNode in adds)
			{
				var assemblyName = addNode?.Attributes?["name"]?.Value;

				if (string.IsNullOrEmpty(assemblyName))
				{
					continue;
				}

				var assembly = currentAssemblies.SingleOrDefault(a => a.GetName().Name == assemblyName);

				if (assembly == null)
				{
					Log.Warn(
						$"Constellation.Foundation.Mvc.Patterns.Configuration: Failed to load assembly name \"{assemblyName}\". for service registration.",
						output);
					continue;
				}

				Log.Debug(
						$"Constellation.Foundation.Mvc.Patterns.Configuration: loaded assembly name \"{assemblyName}\". for service registration.",
						output);
				output.AssembliesToRegister.Add(assembly);
			}

			return output;
		}
		#endregion
	}
}
