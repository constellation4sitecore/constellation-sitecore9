namespace Constellation.Foundation.Mvc
{
	using System.Text;

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

			if (originalStartsWithSlash)
			{
				builder.Append("/");
			}

			for (var i = 0; i < names.Length; i++)
			{
				var name = names[i];

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

		/// <summary>
		/// Takes a Sitecore-compatible phrase (ex: My Item Name) and converts it to a valid C# class name
		/// (ex: MyItemName).
		/// </summary>
		/// <param name="name">The phrase to convert.</param>
		/// <returns>A C# compatible class name.</returns>
		public static string ConvertItemNameToClassName(string name)
		{
			return name.AsClassName();
		}



	}
}
