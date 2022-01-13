using Microsoft.AspNetCore.Mvc.ModelBinding;

using System;

namespace Herald.ModelBinder.RouteAndBody
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public class FromRouteAndBodyAttribute : Attribute, IBindingSourceMetadata
    {
        public BindingSource BindingSource => RouteAndBodyBindingSource.RouteAndBody;
    }
}