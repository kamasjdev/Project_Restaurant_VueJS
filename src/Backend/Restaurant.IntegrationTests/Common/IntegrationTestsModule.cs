using Autofac;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.SqlCommand;
using Restaurant.Infrastructure.IoC;
using System.Diagnostics;

namespace Restaurant.IntegrationTests.Common
{
    internal sealed class IntegrationTestsModule : Module
    {
        private readonly IConfiguration _configuration;

        public IntegrationTestsModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            OverrideSessionFactory(builder);
        }

        private void OverrideSessionFactory(ContainerBuilder builder)
        {
            var mapper = new ModelMapper();
            mapper.AddMappings(typeof(InfrastructureModule).Assembly.ExportedTypes);
            HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            var configuration = new Configuration();
            configuration.DataBaseIntegration(c =>
            {
                c.Dialect<SQLiteDialect>();
                c.ConnectionString = _configuration.GetConnectionString("database");
                c.SchemaAction = SchemaAutoAction.Create;
                c.LogFormattedSql = true;
                c.LogSqlInConsole = true;
            });
            configuration.AddMapping(domainMapping);
            var sessionFactory = configuration.BuildSessionFactory();
            builder.Register(c => sessionFactory).SingleInstance();
        }
    }

    internal sealed class SqlDebugOutputInterceptor : EmptyInterceptor
    {
        public override SqlString OnPrepareStatement(SqlString sql)
        {
            Debug.WriteLine($"NHibernate: {sql}");
            return base.OnPrepareStatement(sql);
        }
    }
}
