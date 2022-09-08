using Microsoft.AspNetCore.Builder;

namespace Restaurant.Infrastructure
{
    public static class Extensions
    {
        public static WebApplication UseInfrastructure(this WebApplication app)
        {
            return app;
        }
    }
}