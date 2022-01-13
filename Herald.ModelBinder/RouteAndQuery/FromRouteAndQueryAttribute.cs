using Microsoft.AspNetCore.Mvc.ModelBinding;

using System;

namespace Herald.ModelBinder.RouteAndQuery
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
	public class FromRouteAndQueryAttribute : Attribute, IBindingSourceMetadata
	{
		public BindingSource BindingSource { get; } = CompositeBindingSource.Create(
			new[] { BindingSource.Path, BindingSource.Query }, nameof(FromRouteAndQueryAttribute));
	}
}
