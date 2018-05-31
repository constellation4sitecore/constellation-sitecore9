using System.Reflection;
using System.Web;

namespace Constellation.Foundation.ModelMapping
{
	public static class PropertyInfoExtensions
	{
		public static bool IsHtml(this PropertyInfo property)
		{
			return typeof(HtmlString).IsAssignableFrom(property.PropertyType);
		}

		public static bool IsString(this PropertyInfo property)
		{
			return typeof(string).IsAssignableFrom(property.PropertyType);
		}

		public static bool Is<T>(this PropertyInfo property)
		{
			return typeof(T).IsAssignableFrom(property.PropertyType);
		}
	}
}
