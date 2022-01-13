using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Herald.ModelBinder.RouteAndBody
{
    public class RouteAndBodyModelBinderProvider : IModelBinderProvider
    {
        private readonly BodyModelBinderProvider _bodyModelBinderProvider;
        private readonly ComplexTypeModelBinderProvider _complexTypeModelBinderProvider;

        public RouteAndBodyModelBinderProvider(BodyModelBinderProvider bodyModelBinderProvider,
            ComplexTypeModelBinderProvider complexObjectModelBinderProvider)
        {
            _bodyModelBinderProvider = bodyModelBinderProvider;
            _complexTypeModelBinderProvider = complexObjectModelBinderProvider;
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            var bodyBinder = _bodyModelBinderProvider.GetBinder(context);
            var complexBinder = _complexTypeModelBinderProvider.GetBinder(context);

            return context.BindingInfo.BindingSource != null
                   && context.BindingInfo.BindingSource.CanAcceptDataFrom(RouteAndBodyBindingSource.RouteAndBody)
                   && bodyBinder != null
                   && complexBinder != null
                ? new RouteAndBodyModelBinder(bodyBinder, complexBinder)
                : default(IModelBinder);
        }
    }
}
