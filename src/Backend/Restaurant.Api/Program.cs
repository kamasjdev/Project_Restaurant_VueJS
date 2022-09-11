using Autofac;
using Autofac.Extensions.DependencyInjection;
using Restaurant.Api;
using Restaurant.Application.IoC;
using Restaurant.Infrastructure;
using Restaurant.Infrastructure.Conventions;
using Restaurant.Infrastructure.IoC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(autofacBuilder =>
{
    autofacBuilder.RegisterModule(new ApplicationModule());
    autofacBuilder.RegisterModule(new InfrastructureModule(builder.Configuration));
});
builder.Services.AddControllers(options =>
{
    options.UseDashedConventionInRouting();
});
builder.Services.AddFluentMigrator(builder.Configuration);
builder.Services.AddEmailSettings(builder.Configuration);
builder.Services.Configure<AppOptions>(builder.Configuration.GetRequiredSection("app"));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseInfrastructure();

app.Run();

public partial class Program { }