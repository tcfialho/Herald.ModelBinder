using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Herald.ModelBinder.RouteAndBody
{
    public class RouteAndBodyBindingSource : BindingSource
    {
        public static readonly BindingSource RouteAndBody = new RouteAndBodyBindingSource(
            "RouteAndBody",
            "RouteAndBody",
            true,
            true
        );

        public RouteAndBodyBindingSource(string id, string displayName, bool isGreedy, bool isFromRequest) : base(id,
            displayName, isGreedy, isFromRequest)
        {
        }

        public override bool CanAcceptDataFrom(BindingSource bindingSource) =>
            bindingSource == Body || bindingSource == this;
    }
}