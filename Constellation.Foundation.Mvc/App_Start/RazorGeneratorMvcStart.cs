using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using RazorGenerator.Mvc;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Constellation.Foundation.Mvc.RazorGeneratorMvcStart), "Start")]

namespace Constellation.Foundation.Mvc
{
	/// <summary>
	/// Ensures that changed View files are recompiled on access.
	/// </summary>
	public static class RazorGeneratorMvcStart
	{
		/// <summary>
		/// The start handler
		/// </summary>
		public static void Start()
		{
			var engine = new PrecompiledMvcEngine(typeof(RazorGeneratorMvcStart).Assembly)
			{
				UsePhysicalViewsIfNewer = HttpContext.Current.Request.IsLocal
			};

			ViewEngines.Engines.Insert(0, engine);

			// StartPage lookups are done by WebPages. 
			VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);
		}
	}
}
