namespace Constellation.Foundation.Items
{
	using Sitecore.Data;
	using Sitecore.Data.Items;
	using Sitecore.Diagnostics;

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using System.Reflection;

	/// <summary>
	/// Translates an Item from a Sitecore object to a strongly typed object
	/// using discovered wrapper classes that map 1:1 with Sitecore data templates.
	/// </summary>
	internal static class ItemFactory
	{
		#region Fields
		/// <summary>
		/// The internal list of candidate types. Do not reference this list directly.
		/// </summary>
		private static IDictionary<ID, Type> candidateClasses;

		/// <summary>
		/// The internal list of candidate types. Do not reference this list directly.
		/// </summary>
		private static IDictionary<ID, Type> candidateInterfaces;
		#endregion

		#region Properties
		/// <summary>
		/// Gets an initialized list of types.
		/// </summary>
		public static IDictionary<ID, Type> CandidateClasses
		{
			get
			{
				return candidateClasses ?? (candidateClasses = CreateCandidateClassesList());
			}
		}

		/// <summary>
		/// Gets an initialized list of types.
		/// </summary>
		public static IDictionary<ID, Type> CandidateInterfaces
		{
			get
			{
				return candidateInterfaces ?? (candidateInterfaces = CreateCandidateInterfacesList());
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Returns the appropriate Type for a given Sitecore Template ID.
		/// </summary>
		/// <param name="id">The ID to look for</param>
		/// <returns>The Type represented by that ID</returns>
		public static Type GetTemplateInterfaceType(ID id)
		{
			if (CandidateInterfaces.ContainsKey(id))
			{
				return CandidateInterfaces[id];
			}

			return null;
		}

		/// <summary>
		/// Wraps the supplied Item with a class that represents the Item's data template.
		/// </summary>
		/// <param name="item">The Item to wrap.</param>
		/// <returns>An instance of IStandardTemplateItem or null if the Item cannot be cast successfully.</returns>
		internal static StandardTemplate GetStronglyTypedItem(Item item)
		{
			Assert.IsNotNull(item, "item cannot be null.");
			Type type;
			if (CandidateClasses.TryGetValue(item.TemplateID, out type))
			{
				return Activator.CreateInstance(type, new object[] { item }) as StandardTemplate;
			}

			return new StandardTemplate(item);
		}
		#endregion

		#region Main Logic
		/// <summary>
		/// Crawls all available assemblies looking for Classes that represent Sitecore
		/// Data Templates.
		/// </summary>
		/// <returns>A dictionary of possible classes, keyed by the Sitecore Template IDs that they match to.</returns>
		private static IDictionary<ID, Type> CreateCandidateClassesList()
		{
			var list = new Dictionary<ID, Type>();

			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

			foreach (Assembly assembly in assemblies)
			{
				var types = GetLoadableTypes(assembly);

				foreach (Type type in types)
				{
					if (type.IsClass)
					{
						ID templateID = GetTemplateIDFromAttribute(type);

						if (!ID.IsNullOrEmpty(templateID))
						{
							Log.Info("ItemFactory added: " + templateID, type);
							list.Add(templateID, type);
						}
					}
				}
			}

			return list;
		}

		/// <summary>
		/// Crawls all available assemblies looking for Interfaces that represent Sitecore
		/// Data Templates.
		/// </summary>
		/// <returns>A dictionary of possible interfaces, keyed by the Sitecore Template IDs that they match to.</returns>
		private static IDictionary<ID, Type> CreateCandidateInterfacesList()
		{
			var list = new Dictionary<ID, Type>();

			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

			foreach (Assembly assembly in assemblies)
			{
				var types = new List<Type>();

				foreach (var type in GetLoadableTypes(assembly))
				{
					try
					{
						if (type.IsInterface)
						{
							types.Add(type);
						}
					}
#pragma warning disable 168
					// ReSharper disable UnusedVariable
					catch (ReflectionTypeLoadException e)
					// ReSharper restore UnusedVariable
#pragma warning restore 168
					{
						// We can't use that particular type.
					}
				}

				foreach (Type type in types)
				{
					ID templateID = GetTemplateIDFromAttribute(type);

					if (!ID.IsNullOrEmpty(templateID))
					{
						list.Add(templateID, type);
					}
				}
			}

			return list;
		}
		#endregion

		#region Helpers
		/// <summary>
		/// Inspects the attribute of the provided type to determine its 
		/// association with a Sitecore template type.
		/// </summary>
		/// <param name="type">The type to inspect.</param>
		/// <returns>The ID of the related Sitecore Template.</returns>
		private static ID GetTemplateIDFromAttribute(Type type)
		{
			try
			{
				// Get the ID property
				var attributes = type.GetCustomAttributes(typeof(TemplateIDAttribute), false);

				if (attributes.Length > 0)
				{
					var attribute = attributes[0] as TemplateIDAttribute;

					if (attribute != null && !attribute.ID.IsNull)
					{
						return attribute.ID;
					}
				}
			}
			catch
			{
				return null;
			}

			return null;
		}

		/// <summary>
		/// Inspects the provided assembly and returns only a list of types that are capable
		/// of being loaded by the current application.
		/// </summary>
		/// <remarks>
		/// See http://haacked.com/archive/2012/07/23/get-all-types-in-an-assembly.aspx for 
		/// details on why this is required.
		/// </remarks>
		/// <param name="assembly">The assembly to parse.</param>
		/// <returns>A list of loadable types.</returns>
		[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Stylecop hates URLs.")]
		private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
		{
			try
			{
				return assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException e)
			{
				return e.Types.Where(t => t != null);
			}
		}
		#endregion
	}
}
