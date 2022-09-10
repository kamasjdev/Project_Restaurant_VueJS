using Microsoft.Extensions.DependencyInjection;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Domain.ValueObjects;
using Restaurant.IntegrationTests.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Restaurant.IntegrationTests.Repositories
{
    public class ProductSaleRepositoryTests : BaseIntegrationTest
    {
        [Fact]
        public async Task should_add_product_sale()
        {
            var product = new Product(Guid.NewGuid(), "Product#1", 100, ProductKind.MainDish);
            var productSale = new ProductSale(Guid.NewGuid(), product, ProductSaleState.New, Email.Of("email@email.com"));
            await _productRepository.AddAsync(product);

            await _productSaleRepository.AddAsync(productSale);

            var productSaleAdded = await _productSaleRepository.GetAsync(productSale.Id);
            Assert.NotNull(productSaleAdded);
            Assert.Equal(productSale.EndPrice.Value, productSaleAdded.EndPrice.Value);
            Assert.Equal(productSale.ProductSaleState, productSaleAdded.ProductSaleState);
            Assert.Equal(productSale.Email, productSaleAdded.Email);
            Assert.NotNull(productSaleAdded.Product);
            Assert.Equal(product.Id.Value, productSaleAdded.ProductId.Value);
            Assert.Null(productSaleAdded.Order);
        }

        [Fact]
        public async Task should_update_product_sale()
        {
            var productSale = await AddDefaultProductSaleAsync();
            var addition = await AddDefaultAdditionAsync();
            var order = await AddDefaultOrderAsync();
            productSale.ChangeAddition(addition);
            productSale.ChangeEmail(Email.Of("test@test1234.com"));
            productSale.AddOrder(order);

            await _productSaleRepository.UpdateAsync(productSale);

            var productSaleUpdated = await _productSaleRepository.GetAsync(productSale.Id);
            Assert.NotNull(productSaleUpdated);
            Assert.Equal(productSale.AdditionId.Value, productSaleUpdated.AdditionId.Value);
            Assert.Equal(productSale.Email, productSaleUpdated.Email);
            Assert.Equal(productSale.OrderId.Value, productSaleUpdated.OrderId.Value);
            Assert.NotNull(productSaleUpdated.Addition);
            Assert.Equal(productSale.Addition.Id.Value, productSaleUpdated.Addition.Id.Value);
            Assert.NotNull(productSaleUpdated.Order);
            Assert.Equal(productSale.Order.Id.Value, productSaleUpdated.Order.Id.Value);
        }

        [Fact]
        public async Task should_delete_order()
        {
            var productSale = await AddDefaultProductSaleAsync();

            await _productSaleRepository.DeleteAsync(productSale);

            var productSaleDeleted = await _productSaleRepository.GetAsync(productSale.Id);
            Assert.Null(productSaleDeleted);
        }

        [Fact]
        public async Task should_get_all_product_sales()
        {
            await AddDefaultProductSaleAsync();
            await AddDefaultProductSaleAsync();

            var productSales = await _productSaleRepository.GetAllAsync();

            Assert.NotNull(productSales);
            Assert.NotEmpty(productSales);
            Assert.True(productSales.Count() > 0);
        }

        private async Task<ProductSale> AddDefaultProductSaleAsync()
        {
            var product = new Product(Guid.NewGuid(), "Product#1", 100, ProductKind.MainDish);
            var productSale = new ProductSale(Guid.NewGuid(), product, ProductSaleState.New, Email.Of("email@email.com"));
            await _productRepository.AddAsync(product);
            await _productSaleRepository.AddAsync(productSale);
            return productSale;
        }

        private async Task<Addition> AddDefaultAdditionAsync()
        {
            var addition = new Addition(Guid.NewGuid(), $"Addition{Guid.NewGuid()}", 20, ProductKind.Drink);
            await _additionRepository.AddAsync(addition);
            return addition;
        }

        private async Task<Order> AddDefaultOrderAsync()
        {
            var product = new Product(Guid.NewGuid(), $"Product-{Guid.NewGuid()}", 100, ProductKind.MainDish);
            var productSale = new ProductSale(Guid.NewGuid(), product, ProductSaleState.New, Email.Of("email@email.com"));
            await _productRepository.AddAsync(product);
            await _productSaleRepository.AddAsync(productSale);
            var order = new Order(Guid.NewGuid(), Guid.NewGuid().ToString(), DateTime.UtcNow, 100, productSale.Email, "this is notes");
            order.AddProduct(productSale);
            await _orderRepository.AddAsync(order);
            return order;
        }

        private readonly IProductRepository _productRepository;
        private readonly IProductSaleRepository _productSaleRepository;
        private readonly IAdditonRepository _additionRepository;
        private readonly IOrderRepository _orderRepository;

        public ProductSaleRepositoryTests(TestApplicationFactory<Program> factory)
        {
            _productRepository = factory.Services.GetRequiredService<IProductRepository>();
            _productSaleRepository = factory.Services.GetRequiredService<IProductSaleRepository>();
            _additionRepository = factory.Services.GetRequiredService<IAdditonRepository>();
            _orderRepository = factory.Services.GetRequiredService<IOrderRepository>();
        }
    }
}
