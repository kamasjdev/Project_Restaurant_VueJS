using Humanizer;
using Microsoft.AspNetCore.Routing;

namespace Restaurant.Infrastructure.Conventions
{
    internal sealed class DashedConvention : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value)
        {
            if (value == null) { return null; }

            var routeName = value.ToString().Kebaberize();

            return routeName;
        }
    }
}
