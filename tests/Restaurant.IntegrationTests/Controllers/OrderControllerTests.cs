using Flurl.Http;
using Restaurant.Application.DTO;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Domain.ValueObjects;
using Restaurant.IntegrationTests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Restaurant.IntegrationTests.Controllers
{
    public class OrderControllerTests : BaseIntegrationTest
    {
        [Fact]
        public async Task should_add_order_and_return_201_with_id()
        {
            var productSale = await AddDefaultProductSaleAsync();
            var dto = new AddOrderDto { Note = "note#1", Email = productSale.Email.Value, ProductSaleIds = new List<Guid> { productSale.Id } };

            var response = await _client.Request(Path).PostJsonAsync(dto);

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe((int)HttpStatusCode.Created);
            var id = GetIdFromHeader(response, Path);
            id.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public async Task should_add_order_and_return_from_db()
        {
            var productSale = await AddDefaultProductSaleAsync();
            var dto = new AddOrderDto { Note = "note#1", Email = productSale.Email.Value, ProductSaleIds = new List<Guid> { productSale.Id } };

            var response = await _client.Request(Path).PostJsonAsync(dto);

            response.ShouldNotBeNull();
            var id = GetIdFromHeader(response, Path);
            var orderAdded = await _client.Request($"{Path}/{id}").GetJsonAsync<OrderDetailsDto>();
            orderAdded.ShouldNotBeNull();
            orderAdded.Id.ShouldBe(id);
            orderAdded.Price.ShouldBe(productSale.EndPrice.Value);
            orderAdded.Note.ShouldBe(dto.Note);
            orderAdded.Products.Count().ShouldBe(dto.ProductSaleIds.Count());
        }

        [Fact]
        public async Task should_update_order_and_return_staus_code_204()
        {
            var order = await AddDefaultOrderAsync();
            var dto = new AddOrderDto { Email = order.Email + "ab", Note = "note abc 123", ProductSaleIds = order.Products.Select(p => p.Id.Value) };

            var response = await _client.Request($"{Path}/{order.Id.Value}").PutJsonAsync(dto);

            response.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task should_update_order_and_return_from_db()
        {
            var order = await AddDefaultOrderAsync();
            var productSaleIds = order.Products.Select(p => p.Id.Value).ToList();
            var productSale = await AddDefaultProductSaleAsync();
            productSaleIds.Add(productSale.Id);
            var dto = new AddOrderDto { Email = order.Email + "ab", Note = "note abc 123", ProductSaleIds = productSaleIds };

            await _client.Request($"{Path}/{order.Id.Value}").PutJsonAsync(dto);

            var orderUpdated = await _client.Request($"{Path}/{order.Id.Value}").GetJsonAsync<OrderDetailsDto>();
            orderUpdated.ShouldNotBeNull();
            orderUpdated.Id.ShouldBe(order.Id.Value);
            orderUpdated.Note.ShouldBe(dto.Note);
            orderUpdated.Email.ShouldBe(dto.Email);
            orderUpdated.Products.Count().ShouldBe(productSaleIds.Count);
            orderUpdated.Price.ShouldBe(order.Price.Value + productSale.EndPrice.Value);
        }

        [Fact]
        public async Task should_delete_order_and_return_200()
        {
            var order = await AddDefaultOrderAsync();

            await _client.Request($"{Path}/{order.Id.Value}").DeleteAsync();

            var orderDeleted = await _orderRepository.GetAsync(order.Id.Value);
            orderDeleted.ShouldBeNull();
        }

        [Fact]
        public async Task should_get_order_and_return_200()
        {
            var order = await AddDefaultOrderAsync();

            var response = await _client.Request($"{Path}/{order.Id.Value}").GetAsync();
            var dto = await response.ResponseMessage.Content.ReadFromJsonAsync<OrderDetailsDto>();

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(order.Id.Value);
            dto.Price.ShouldBe(order.Price.Value);
            dto.OrderNumber.ShouldBe(order.OrderNumber.Value);
            dto.Email.ShouldBe(order.Email.Value);
            dto.Note.ShouldBe(order.Note);
            dto.Created.ShouldBe(order.Created);
            dto.Products.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task should_get_all_orders()
        {
            await AddDefaultOrderAsync();
            await AddDefaultOrderAsync();
            await AddDefaultOrderAsync();

            var response = await _client.Request(Path).GetAsync();
            var dtos = await response.ResponseMessage.Content.ReadFromJsonAsync<IEnumerable<OrderDto>>();

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            dtos.ShouldNotBeNull();
            dtos.ShouldNotBeEmpty();
            dtos.Count().ShouldBeGreaterThan(2);
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

        private async Task<ProductSale> AddDefaultProductSaleAsync()
        {
            var product = await AddDefaultProductAsync();
            var addition = await AddDefaultAdditionAsync();
            var productSale = new ProductSale(Guid.NewGuid(), product, ProductSaleState.New, Email.Of("test@test.com"), addition);
            await _productSaleRepository.AddAsync(productSale);
            return productSale;
        }

        private async Task<Product> AddDefaultProductAsync()
        {
            var random = new Random();
            var cost = random.Next(10, 200);
            var productKind = random.Next(0, 3) switch
            {
                0 => ProductKind.Pizza,
                1 => ProductKind.MainDish,
                2 => ProductKind.Soup,
                _ => ProductKind.Pizza
            };
            var product = new Product(Guid.NewGuid(), $"Product{Guid.NewGuid().ToString("N")}", cost, productKind);
            await _productRepository.AddAsync(product);
            return product;
        }

        private async Task<Addition> AddDefaultAdditionAsync()
        {
            var random = new Random();
            var cost = random.Next(1, 20);
            var additionKind = random.Next(0, 2) == 1 ? AdditionKind.Salad : AdditionKind.Drink;
            var addition = new Addition(Guid.NewGuid(), $"Addition{Guid.NewGuid().ToString("N")}", cost, additionKind);
            await _additionRepository.AddAsync(addition);
            return addition;
        }

        private const string Path = "/api/orders";
        private readonly IOrderRepository _orderRepository;
        private readonly IProductSaleRepository _productSaleRepository;
        private readonly IProductRepository _productRepository;
        private IAdditonRepository _additionRepository;

        public OrderControllerTests(TestApplicationFactory<Program> factory) : base(factory)
        {
            _orderRepository = GetRequiredService<IOrderRepository>();
            _productSaleRepository = GetRequiredService<IProductSaleRepository>();
            _productRepository = GetRequiredService<IProductRepository>();
            _additionRepository = GetRequiredService<IAdditonRepository>();
        }
    }
}
