using Herald.ModelBinder.RouteAndBody;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

using System.Collections.Generic;
using System.Linq;

namespace Herald.ModelBinder
{
    public static class ModelBinderProviderExtensions
    {
        public static IList<IModelBinderProvider> InsertRouteAndQueryBinding(this IList<IModelBinderProvider> providers)
        {
            return providers;
        }

        public static IList<IModelBinderProvider> InsertRouteAndBodyBinding(this IList<IModelBinderProvider> providers)
        {
            var bodyProvider = providers.OfType<BodyModelBinderProvider>().Single();
            var complexProvider = providers.OfType<ComplexTypeModelBinderProvider>().FirstOrDefault() ?? new ComplexTypeModelBinderProvider();

            var RouteAndBodyProvider = new RouteAndBodyModelBinderProvider(bodyProvider, complexProvider);

            providers.Insert(0, RouteAndBodyProvider);
            return providers;
        }
    }
}