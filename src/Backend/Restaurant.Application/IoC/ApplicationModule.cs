using Autofac;
using Restaurant.Application.Abstractions;
using Restaurant.Application.Mail;
using Restaurant.Application.Services;
using Restaurant.Application.Time;

namespace Restaurant.Application.IoC
{
    public sealed class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AdditionService>().As<IAdditionService>().InstancePerLifetimeScope();
            builder.RegisterType<OrderService>().As<IOrderService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductService>().As<IProductService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductSaleService>().As<IProductSaleService>().InstancePerLifetimeScope();
            builder.RegisterType<MailSender>().As<IMailSender>().InstancePerLifetimeScope();
            builder.RegisterType<Clock>().As<IClock>().SingleInstance();

            builder.RegisterType<MailSender>().As<IMailSender>().InstancePerLifetimeScope();
            builder.RegisterType<SmtpClientWrapper>().As<ISmtpClient>().InstancePerLifetimeScope();
        }
    }
}
