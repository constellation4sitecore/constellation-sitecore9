using Sitecore;
using Sitecore.Mvc.Pipelines;
using Sitecore.Mvc.Pipelines.Response.GetPageItem;

namespace Constellation.Foundation.PageNotFound.Pipelines.Mvc
{
    public class CheckItemResolved : MvcPipelineProcessor<GetPageItemArgs>
    {
        public override void Process(GetPageItemArgs args)
        {
            var resolved = Sitecore.Context.Items["constellation4sitecore.Foundation.PageNotFound::ItemNotFoundResolved"];
            if (MainUtil.GetBool(resolved, false))
            {
                // item has previously been resolved
                args.Result = Sitecore.Context.Item;
            }
        }
    }
}
