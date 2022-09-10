using Microsoft.AspNetCore.Builder;

namespace Restaurant.Infrastructure.Exceptions
{
    internal static class Extensions
    {
        public static WebApplication UseErrorHandling(this WebApplication app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
            return app;
        }
    }
}