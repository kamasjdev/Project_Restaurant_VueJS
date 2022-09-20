using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Application.Mail;
using Restaurant.Infrastructure.Exceptions;
using Restaurant.Infrastructure.Initializers;
using Restaurant.Infrastructure.Security;
using Restaurant.Migrations;

namespace Restaurant.Infrastructure
{
    public static class Extensions
    {
        private const string CorsPolicy = "cors";

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(cors =>
            {
                cors.AddPolicy(CorsPolicy, policy =>
                {
                    policy.WithOrigins("*")
                          .WithMethods("POST", "PUT", "PATCH", "DELETE")
                          .WithHeaders("Content-Type", "Authorization")
                          .WithExposedHeaders("Location");
                });
            });
            services.AddEmailSettings(configuration);
            services.AddFluentMigrator(configuration);
            services.AddAuth(configuration);
            return services;
        }

        private static IServiceCollection AddEmailSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetRequiredSection("emailSettings"));
            return services;
        }

        private static IServiceCollection AddFluentMigrator(this IServiceCollection services, IConfiguration configuration)
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
            app.UseCors(CorsPolicy);
            app.UseErrorHandling();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            return app;
        }

        public static T GetOptions<T>(this IConfiguration configuration, string sectionName)
            where T : class, new()
        {
            var options = new T();
            var section = configuration.GetRequiredSection(sectionName);
            section.Bind(options);
            return options;
        }
    }
}