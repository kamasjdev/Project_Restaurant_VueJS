using Autofac;
using Restaurant.Domain.Repositories;
using Restaurant.Infrastructure.Repositories;

namespace Restaurant.Infrastructure.IoC
{
    public sealed class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AdditionRepository>().As<IAdditonRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ProductRepository>().As<IProductRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ProductSaleRepository>().As<IProductSaleRepository>().InstancePerLifetimeScope();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>().InstancePerLifetimeScope();
        }
    }
}
