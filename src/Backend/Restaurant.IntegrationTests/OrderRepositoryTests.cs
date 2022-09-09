using Microsoft.Extensions.DependencyInjection;
using Restaurant.Domain.Repositories;
using Restaurant.IntegrationTests.Common;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Restaurant.IntegrationTests
{
    [Collection("TestCollection")]
    public class OrderRepositoryTests
    {
        [Fact]
        public async Task should_add_order()
        {
            var orderRepository = _services.GetRequiredService<IOrderRepository>();
            var productRepository = _services.GetRequiredService<IProductRepository>();
            var productSaleRepository = _services.GetRequiredService<IProductSaleRepository>();
        }

        private readonly IServiceProvider _services;

        public OrderRepositoryTests(TestApplicationFactory<Program> factory)
        {
            _services = factory.Services;
        }
    }
}
