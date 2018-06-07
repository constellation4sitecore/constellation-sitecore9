namespace Constellation.Foundation.Mvc
{
	using System.Text;
	using Constellation.Foundation.Data;

	/// <summary>
	/// Small utility to convert Sitecore Item names to C# compatible names in a variety of scenarios.
	/// </summary>
	public class NameConverter
	{
		/// <summary>
		/// Takes a Sitecore URL ex:(/sitecore/layout/renderings/my rendering name) and converts it to a
		/// file path that could also be a C# namespace (ex: /Sitecore/Layout/Renderings/MyRenderingName)
		/// </summary>
		/// <param name="path">The Sitecore URL to convert.</param>
		/// <returns>a C# compatible file path.</returns>
		public static string ConvertItemPathToClassPath(string path)
		{
			var names = path.Split('/');
			var originalStartsWithSlash = path.StartsWith("/");
			var originalEndsWithSlash = path.EndsWith("/");

			var builder = new StringBuilder();

			for (var i = 0; i < names.Length; i++)
			{
				var name = names[i];

				if (string.IsNullOrEmpty(name))
				{
					continue;
				}

				if (i > 0 || originalStartsWithSlash)
				{
					builder.Append("/");
				}

				builder.Append(name.AsClassName());
			}

			if (originalEndsWithSlash)
			{
				builder.Append("/");
			}

			return builder.ToString();
		}
	}
}
