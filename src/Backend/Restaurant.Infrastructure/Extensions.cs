using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Application.Mail;
using Restaurant.Infrastructure.Exceptions;
using Restaurant.Infrastructure.Initializers;
using Restaurant.Migrations;

namespace Restaurant.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddEmailSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetRequiredSection("emailSettings"));
            return services;
        }

        public static IServiceCollection AddFluentMigrator(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFluentMigratorCore()
                .ConfigureRunner(cr =>
                            cr.AddSQLite()
                               .WithGlobalConnectionString(configuration.GetConnectionString("database"))
                               .ScanIn(typeof(AddAdditionTable).Assembly).For.Migrations())
                .AddLogging(l => l.AddFluentMigratorConsole());
            services.AddHostedService<DbInitializer>();
            return services;
        }

        public static WebApplication UseInfrastructure(this WebApplication app)
        {
            app.UseAuthorization();
            app.UseErrorHandling();
            app.MapControllers();
            return app;
        }
    }
}