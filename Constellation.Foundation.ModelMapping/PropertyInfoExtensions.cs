using System.Reflection;
using System.Web;

namespace Constellation.Foundation.ModelMapping
{
	/// <summary>
	/// A series of extensions to the PropertyInfo class in Reflection to make certain Mapper code more legible.
	/// </summary>
	public static class PropertyInfoExtensions
	{
		/// <summary>
		/// Is the property being referenced an HtmlString?
		/// </summary>
		/// <param name="property">The property to inspect.</param>
		/// <returns>True if the property is an HtmlString.</returns>
		public static bool IsHtml(this PropertyInfo property)
		{
			return typeof(HtmlString).IsAssignableFrom(property.PropertyType);
		}

		/// <summary>
		/// Is the property being referenced a String?
		/// </summary>
		/// <param name="property">The property to inspect.</param>
		/// <returns>True if the property is a String.</returns>
		public static bool IsString(this PropertyInfo property)
		{
			return typeof(string).IsAssignableFrom(property.PropertyType);
		}

		/// <summary>
		/// Is the property of the following type "T"?
		/// </summary>
		/// <param name="property">The property to inspect.</param>
		/// <typeparam name="T">The Type that the property should be tested for assignment.</typeparam>
		/// <returns>True if the property is assignable to "T".</returns>
		public static bool Is<T>(this PropertyInfo property)
		{
			return typeof(T).IsAssignableFrom(property.PropertyType);
		}
	}
}
