using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Restaurant.Infrastructure.Conventions
{
    public static class Extensions
    {
        public static MvcOptions UseDashedConventionInRouting(this MvcOptions options)
        {
            options.Conventions.Add(new RouteTokenTransformerConvention(new DashedConvention()));
            return options;
        }
    }
}
