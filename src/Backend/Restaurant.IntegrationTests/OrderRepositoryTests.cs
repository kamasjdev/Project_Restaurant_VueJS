using Microsoft.Extensions.DependencyInjection;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Domain.ValueObjects;
using Restaurant.IntegrationTests.Common;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Restaurant.IntegrationTests
{
    [Collection("TestCollection")]
    public class OrderRepositoryTests
    {
        [Fact]
        public async Task should_add_order()
        {
            var product = new Product(Guid.NewGuid(), "Product#1", 100, ProductKind.MainDish);
            var productSale = new ProductSale(Guid.NewGuid(), product, ProductSaleState.New, Email.Of("email@email.com"));
            await _productRepository.AddAsync(product);
            await _productSaleRepository.AddAsync(productSale);
            var order = new Order(Guid.NewGuid(), Guid.NewGuid().ToString(), DateTime.UtcNow, 100, productSale.Email, "this is notes");
            order.AddProduct(productSale);

            await _orderRepository.AddAsync(order);

            var orderAdded = await _orderRepository.GetAsync(order.Id);
            Assert.NotNull(orderAdded);
        }

        [Fact]
        public async Task should_update_order()
        {
            var product = new Product(Guid.NewGuid(), "Product#1", 100, ProductKind.MainDish);
            var productSale = new ProductSale(Guid.NewGuid(), product, ProductSaleState.New, Email.Of("email@email.com"));
            await _productRepository.AddAsync(product);
            await _productSaleRepository.AddAsync(productSale);
            var order = new Order(Guid.NewGuid(), Guid.NewGuid().ToString(), DateTime.UtcNow, 100, productSale.Email, "this is notes");
            order.AddProduct(productSale);
            await _orderRepository.AddAsync(order);
            order.Note += " notes again";
            var productSale2 = new ProductSale(Guid.NewGuid(), product, ProductSaleState.New, Email.Of("email@email.com"));
            await _productSaleRepository.AddAsync(productSale2);
            order.AddProduct(productSale2);

            await _orderRepository.UpdateAsync(order);

            var orderAdded = await _orderRepository.GetAsync(order.Id);
            Assert.NotNull(orderAdded);
        }

        [Fact]
        public async Task should_delete_order()
        {
            var product = new Product(Guid.NewGuid(), "Product#1", 100, ProductKind.MainDish);
            var productSale = new ProductSale(Guid.NewGuid(), product, ProductSaleState.New, Email.Of("email@email.com"));
            await _productRepository.AddAsync(product);
            await _productSaleRepository.AddAsync(productSale);
            var order = new Order(Guid.NewGuid(), Guid.NewGuid().ToString(), DateTime.UtcNow, 100, productSale.Email, "this is notes");
            order.AddProduct(productSale);
            await _orderRepository.AddAsync(order);

            await _orderRepository.DeleteAsync(order);

            var orderAdded = await _orderRepository.GetAsync(order.Id);
            Assert.Null(orderAdded);
        }

        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductSaleRepository _productSaleRepository;
        private readonly ITestOutputHelper _output;

        public OrderRepositoryTests(TestApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _orderRepository = factory.Services.GetRequiredService<IOrderRepository>();
            _productRepository = factory.Services.GetRequiredService<IProductRepository>();
            _productSaleRepository = factory.Services.GetRequiredService<IProductSaleRepository>();
            _output = output;
        }

        public class ConsoleWriter : StringWriter
        {
            private ITestOutputHelper output;
            public ConsoleWriter(ITestOutputHelper output)
            {
                this.output = output;
            }

            public override void WriteLine(string m)
            {
                output.WriteLine(m);
            }
        }
    }
}
