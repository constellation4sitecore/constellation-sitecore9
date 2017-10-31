namespace Constellation.Foundation.Mvc.Pipelines.GetRenderer
{
	using Sitecore.Data.Items;
	using Sitecore.Data.Templates;
	using Sitecore.Mvc.Configuration;
	using Sitecore.Mvc.Extensions;
	using Sitecore.Mvc.Names;
	using Sitecore.Mvc.Pipelines.Response.GetRenderer;
	using Sitecore.Mvc.Presentation;
	using System;

	public class GetControllerRenderer : GetRendererProcessor
	{
		public override void Process(GetRendererArgs args)
		{
			if (args.Result != null)
				return;
			args.Result = this.GetRenderer(args.Rendering, args);
		}

		protected virtual Tuple<string, string> GetControllerAndAction(Rendering rendering, GetRendererArgs args)
		{
			Tuple<string, string> tuple = GetFromProperties(rendering) ?? GetFromRenderingItem(rendering, args);
			if (tuple == null)
			{
				return null;
			}
			return MvcSettings.ControllerLocator.GetControllerAndAction(tuple.Item1, tuple.Item2);
		}

		protected virtual Renderer GetRenderer(Rendering rendering, GetRendererArgs args)
		{
			Tuple<string, string> controllerAndAction = this.GetControllerAndAction(rendering, args);
			if (controllerAndAction == null)
				return null;
			string str1 = controllerAndAction.Item1;
			string str2 = controllerAndAction.Item2;
			return new ControllerRenderer()
			{
				ControllerName = str1,
				ActionName = str2
			};
		}

		private Tuple<string, string> GetFromProperties(Rendering rendering)
		{
			if (rendering.RenderingType != "Controller")
			{
				return null;
			}
			string controllerName = rendering["Controller"];
			string actionName = rendering["Controller Action"];

			if (controllerName.IsWhiteSpaceOrNull())
			{
				controllerName = rendering.RenderingItem.Name;
			}

			return new Tuple<string, string>(controllerName, actionName);
		}

		private Tuple<string, string> GetFromRenderingItem(Rendering rendering, GetRendererArgs args)
		{
			Template renderingTemplate = args.RenderingTemplate;
			if (renderingTemplate == null)
			{
				return null;
			}

			if (!renderingTemplate.DescendsFromOrEquals(TemplateIds.ControllerRendering))
			{
				return null;
			}

			RenderingItem renderingItem = rendering.RenderingItem;
			if (renderingItem == null)
			{
				return null;
			}

			string controllerName = renderingItem.InnerItem["Controller"];
			string actionName = renderingItem.InnerItem["Controller Action"];

			if (controllerName.IsWhiteSpaceOrNull())
			{
				controllerName = NameConverter.ConvertItemNameToClassName(renderingItem.Name);
			}

			return new Tuple<string, string>(controllerName, actionName);
		}
	}
}
