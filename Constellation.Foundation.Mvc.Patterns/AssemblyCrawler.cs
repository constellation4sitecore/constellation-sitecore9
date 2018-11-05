using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Constellation.Foundation.Mvc.Patterns
{
	internal static class AssemblyCrawler
	{
		internal static Type[] GetTypesImplementing<T>(params Assembly[] assemblies)
		{
			if (assemblies == null || assemblies.Length == 0)
			{
				return new Type[0];
			}

			var targetType = typeof(T);

			return assemblies
				.Where(assembly => !assembly.IsDynamic && !assembly.FullName.StartsWith("Sitecore"))
				.SelectMany(GetExportedTypes)
				.Where(type => !type.IsInterface && !type.IsAbstract && !type.IsGenericTypeDefinition && targetType.IsAssignableFrom(type))
				.ToArray();
		}

		private static IEnumerable<Type> GetExportedTypes(Assembly assembly)
		{
			try
			{
				return assembly.GetExportedTypes();
			}
			catch (FileLoadException)
			{
				// Probably a type version mismatch, likely not custom code and can be skipped reliably
				return Type.EmptyTypes;
			}
			catch (TypeLoadException)
			{
				// Garbage, we can reliably skip this.
				return Type.EmptyTypes;
			}
			catch (NotSupportedException)
			{
				// A type load exception would typically happen on an Anonymously Hosted DynamicMethods
				// Assembly and it would be safe to skip this exception.
				return Type.EmptyTypes;
			}
			catch (ReflectionTypeLoadException ex)
			{
				// Return the types that could be loaded. Types can contain null values.
				return ex.Types.Where(type => type != null);
			}
			catch (Exception ex)
			{
				// Throw a more descriptive message containing the name of the assembly.
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
					"Unable to load types from assembly {0}. {1}", assembly.FullName, ex.Message), ex);
			}
		}
	}
}
