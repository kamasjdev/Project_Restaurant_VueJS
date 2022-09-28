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
    public class OrderRepositoryTests : BaseIntegrationTest
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
            Assert.Equal(order.Email, orderAdded.Email);
            Assert.Equal(order.Price.Value, orderAdded.Price.Value);
            Assert.Equal(order.Created, orderAdded.Created);
            Assert.Equal(order.OrderNumber.Value, orderAdded.OrderNumber.Value);
            Assert.Equal(order.Note, orderAdded.Note);
            Assert.NotNull(orderAdded.Products);
            Assert.NotEmpty(orderAdded.Products);
            Assert.Equal(1, orderAdded.Products.Count());
        }

        [Fact]
        public async Task should_add_order_with_product_and_addition()
        {
            var product = new Product(Guid.NewGuid(), "Product#1", 100, ProductKind.MainDish);
            var addition = await AddDefaultAdditionAsync();
            var productSale = new ProductSale(Guid.NewGuid(), product, ProductSaleState.New, Email.Of("email@email.com"), addition);
            await _productRepository.AddAsync(product);
            await _productSaleRepository.AddAsync(productSale);
            var order = new Order(Guid.NewGuid(), Guid.NewGuid().ToString(), DateTime.UtcNow, 100, productSale.Email, "this is notes");
            order.AddProduct(productSale);

            await _orderRepository.AddAsync(order);

            var orderAdded = await _orderRepository.GetAsync(order.Id);
            Assert.NotNull(orderAdded);
            Assert.Equal(order.Email, orderAdded.Email);
            Assert.Equal(order.Price.Value, orderAdded.Price.Value);
            Assert.Equal(order.Created, orderAdded.Created);
            Assert.Equal(order.OrderNumber.Value, orderAdded.OrderNumber.Value);
            Assert.Equal(order.Note, orderAdded.Note);
            Assert.NotNull(orderAdded.Products);
            Assert.NotEmpty(orderAdded.Products);
            Assert.Equal(1, orderAdded.Products.Count());
        }

        [Fact]
        public async Task should_update_order()
        {
            var product = new Product(Guid.NewGuid(), "Product#1abc", 100, ProductKind.MainDish);
            await _productRepository.AddAsync(product);
            var order = await AddDefaultOrderAsync();
            order.ChangeNote(order.Note + " notes again");
            var productSale2 = new ProductSale(Guid.NewGuid(), product, ProductSaleState.New, Email.Of("email@email.com"));
            await _productSaleRepository.AddAsync(productSale2);
            order.AddProduct(productSale2);

            await _orderRepository.UpdateAsync(order);

            var orderUpdated = await _orderRepository.GetAsync(order.Id);
            Assert.NotNull(orderUpdated);
            Assert.Equal(order.Note, orderUpdated.Note);
            Assert.NotEmpty(orderUpdated.Products);
            Assert.Equal(2, orderUpdated.Products.Count());
        }

        [Fact]
        public async Task should_delete_order()
        {
            var order = await AddDefaultOrderAsync();

            await _orderRepository.DeleteAsync(order);

            var orderDeleted = await _orderRepository.GetAsync(order.Id);
            Assert.Null(orderDeleted);
        }

        [Fact]
        public async Task should_get_all_orders()
        {
            await AddDefaultOrderAsync();
            await AddDefaultOrderAsync();

            var orders = await _orderRepository.GetAllAsync();

            Assert.NotNull(orders);
            Assert.NotEmpty(orders);
            Assert.True(orders.Count() > 0);
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

        private async Task<Addition> AddDefaultAdditionAsync()
        {
            var addition = new Addition(Guid.NewGuid(), $"Addition{Guid.NewGuid()}", 20, AdditionKind.Drink);
            await _additionRepository.AddAsync(addition);
            return addition;
        }

        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductSaleRepository _productSaleRepository;
        private readonly IAdditonRepository _additionRepository;

        public OrderRepositoryTests(TestApplicationFactory<Program> factory) : base(factory)
        {
            _orderRepository = GetRequiredService<IOrderRepository>();
            _productRepository = GetRequiredService<IProductRepository>();
            _productSaleRepository = GetRequiredService<IProductSaleRepository>();
            _additionRepository = GetRequiredService<IAdditonRepository>();
        }
    }
}
