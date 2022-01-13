using Microsoft.AspNetCore.Mvc.ModelBinding;

using System.Threading.Tasks;

namespace Herald.ModelBinder.RouteAndBody
{
    public class RouteAndBodyModelBinder : IModelBinder
    {
        private readonly IModelBinder _bodyBinder;
        private readonly IModelBinder _complexBinder;

        public RouteAndBodyModelBinder(IModelBinder bodyBinder, IModelBinder complexBinder)
        {
            _bodyBinder = bodyBinder;
            _complexBinder = complexBinder;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            await _bodyBinder.BindModelAsync(bindingContext);

            if (bindingContext.Result.IsModelSet) bindingContext.Model = bindingContext.Result.Model;
            else bindingContext.ModelState.Remove(string.Empty);

            await _complexBinder.BindModelAsync(bindingContext);
        }
    }
}