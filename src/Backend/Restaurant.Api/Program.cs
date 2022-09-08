using Autofac;
using Autofac.Extensions.DependencyInjection;
using Restaurant.Application.IoC;
using Restaurant.Infrastructure;
using Restaurant.Infrastructure.IoC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(autofacBuilder =>
{
    autofacBuilder.RegisterModule(new ApplicationModule());
    autofacBuilder.RegisterModule(new InfrastructureModule(builder.Configuration));
});
builder.Services.AddControllers();
builder.Services.AddFluentMigrator(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
