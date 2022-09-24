using Autofac;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.SqlCommand;
using Restaurant.Application.Abstractions;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Infrastructure.Exceptions;
using Restaurant.Infrastructure.Repositories;
using Restaurant.Infrastructure.Security;
using System.Diagnostics;

namespace Restaurant.Infrastructure.IoC
{
    public sealed class InfrastructureModule : Module
    {
        private readonly IConfiguration _configuration;

        public InfrastructureModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AdditionRepository>().As<IAdditonRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ProductRepository>().As<IProductRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ProductSaleRepository>().As<IProductSaleRepository>().InstancePerLifetimeScope();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>().InstancePerLifetimeScope();
            AddNHbernate(builder);
            builder.RegisterType<ErrorHandlerMiddleware>().AsSelf().SingleInstance();
            builder.RegisterType<PasswordManager>().As<IPasswordManager>().SingleInstance();
            builder.RegisterType<PasswordHasher<User>>().As<IPasswordHasher<User>>().SingleInstance();
            builder.RegisterType<JwtManager>().As<IJwtManager>().SingleInstance();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
        }

        public void AddNHbernate(ContainerBuilder builder)
        {
            var mapper = new ModelMapper();
            mapper.AddMappings(typeof(InfrastructureModule).Assembly.ExportedTypes);
            HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            var configuration = new Configuration();
            configuration.DataBaseIntegration(c =>
            {
                c.Dialect<SQLiteDialect>();
                c.ConnectionString = _configuration.GetConnectionString("database");
                c.LogFormattedSql = true;
                c.LogSqlInConsole = true;
            });
            configuration.AddMapping(domainMapping);           

            var sessionFactory = Fluently.Configure(configuration)
                .Database(SQLiteConfiguration.Standard.ConnectionString(_configuration.GetConnectionString("database")))
                .Mappings(m => m.FluentMappings.AddFromAssembly(typeof(InfrastructureModule).Assembly))
                .BuildSessionFactory();

            builder.Register(c => sessionFactory).SingleInstance();
            builder.Register(c =>
            {
                var interceptor = new SqlDebugOutputInterceptor();
                var session = sessionFactory
                                .WithOptions()
                                .Interceptor(interceptor)
                                .OpenSession();
                return session;
            }).InstancePerLifetimeScope();
        }
    }

    internal sealed class SqlDebugOutputInterceptor : EmptyInterceptor
    {
        public override SqlString OnPrepareStatement(SqlString sql)
        {
            Debug.Write("NHibernate: ");
            Debug.WriteLine(sql);

            return base.OnPrepareStatement(sql);
        }
    }
}
