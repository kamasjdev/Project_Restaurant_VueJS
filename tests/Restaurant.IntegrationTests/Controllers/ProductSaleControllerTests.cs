using Flurl.Http;
using Restaurant.Application.DTO;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Domain.ValueObjects;
using Restaurant.IntegrationTests.Common;
using Shouldly;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Restaurant.IntegrationTests.Controllers
{
    public class ProductSaleControllerTests : BaseIntegrationTest
    {
        [Fact]
        public async Task should_add_product_sale_with_product_and_return_201()
        {
            var product = await AddDefaultProductAsync();
            var email = "test@test.com";
            var dto = new AddProductSaleDto() { Email = email, ProductId = product.Id };

            var response = await _client.Request(Path).PostJsonAsync(dto);

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe((int)HttpStatusCode.Created);
            var id = GetIdFromHeader(response, Path);
            id.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public async Task should_add_product_sale_with_product_and_return_from_db()
        {
            var product = await AddDefaultProductAsync();
            var email = "test@test.com";
            var dto = new AddProductSaleDto() { Email = email, ProductId = product.Id };

            var response = await _client.Request(Path).PostJsonAsync(dto);

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe((int)HttpStatusCode.Created);
            var id = GetIdFromHeader(response, Path);
            id.ShouldNotBe(Guid.Empty);
            var productSaleAdded = await _client.Request($"{Path}/{id}").GetJsonAsync<ProductSaleDto>();
            productSaleAdded.ShouldNotBeNull();
            productSaleAdded.Email.ShouldBe(email);
            productSaleAdded.EndPrice.ShouldBe(product.Price.Value);
        }

        [Fact]
        public async Task should_add_product_sale_with_product_and_Addition_and_return_201()
        {
            var product = await AddDefaultProductAsync();
            var addition = await AddDefaultAdditionAsync();
            var email = "test@test.com";
            var dto = new AddProductSaleDto() { Email = email, ProductId = product.Id, AdditionId = addition.Id };

            var response = await _client.Request(Path).PostJsonAsync(dto);

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe((int)HttpStatusCode.Created);
            var id = GetIdFromHeader(response, Path);
            id.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public async Task should_add_product_sale_with_product_and_Addition_and_return_from_db()
        {
            var product = await AddDefaultProductAsync();
            var addition = await AddDefaultAdditionAsync();
            var email = "test@test.com";
            var dto = new AddProductSaleDto() { Email = email, ProductId = product.Id, AdditionId = addition.Id };

            var response = await _client.Request(Path).PostJsonAsync(dto);

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe((int)HttpStatusCode.Created);
            var id = GetIdFromHeader(response, Path);
            id.ShouldNotBe(Guid.Empty);
            var productSaleAdded = await _client.Request($"{Path}/{id}").GetJsonAsync<ProductSaleDto>();
            productSaleAdded.ShouldNotBeNull();
            productSaleAdded.Email.ShouldBe(email);
            productSaleAdded.EndPrice.ShouldBe(product.Price.Value + addition.Price.Value);
        }

        [Fact]
        public async Task should_update_product_sale_and_return_204()
        {
            var productSale = await AddDefaultProductSaleAsync();
            var dto = new AddProductSaleDto() { Email = "emailtestabc@test.com", ProductId = productSale.Product.Id };

            var response = await _client.Request($"{Path}/{productSale.Id.Value}").PutJsonAsync(dto);

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task should_update_product_sale_and_return_from_db()
        {
            var productSale = await AddDefaultProductSaleAsync();
            var dto = new AddProductSaleDto() { Email = "emailtestabc@test.com", ProductId = productSale.Product.Id };

            var response = await _client.Request($"{Path}/{productSale.Id.Value}").PutJsonAsync(dto);

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
            var productUpdated = await _client.Request($"{Path}/{productSale.Id.Value}").GetJsonAsync<ProductSaleDto>();
            productUpdated.ShouldNotBeNull();
            productUpdated.Email.ShouldBe(dto.Email);
            productUpdated.EndPrice.ShouldBe(productSale.Product.Price.Value);
            productUpdated.Product.Id.ShouldBe(dto.ProductId);
            productUpdated.Addition.ShouldBeNull();
            productUpdated.Order.ShouldBeNull();
        }

        [Fact]
        public async Task should_delete_product_sale_and_return_200()
        {
            var productSale = await AddDefaultProductSaleAsync();
            
            var response = await _client.Request($"{Path}/{productSale.Id.Value}").DeleteAsync();

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            var productSaleDeleted = await _productSaleRepository.GetAsync(productSale.Id.Value);
            productSaleDeleted.ShouldBeNull();
        }

        [Fact]
        public async Task should_get_product_sale_by_id_and_return_200()
        {
            var productSale = await AddDefaultProductSaleAsync();

            var response = await _client.Request($"{Path}/{productSale.Id.Value}").GetAsync();

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            var dto = await response.ResponseMessage.Content.ReadFromJsonAsync<ProductSaleDto>();
            dto.ShouldNotBeNull();
            dto.Email.ShouldBe(productSale.Email.Value);
            dto.Product.ShouldNotBeNull();
            dto.Product.Id.ShouldBe(productSale.ProductId.Value);
            dto.Addition.ShouldNotBeNull();
            dto.Addition.Id.ShouldBe(productSale.AdditionId.Value);
            dto.EndPrice.ShouldBe(productSale.EndPrice.Value);
            dto.ProductSaleState.ShouldBe(productSale.ProductSaleState.ToString());
        }

        [Fact]
        public async Task should_get_product_sales_by_email_and_return_200()
        {
            var productSale = await AddDefaultProductSaleAsync();
            await AddDefaultProductSaleAsync();

            var response = await _client.Request($"{Path}/by-email/{productSale.Email.Value}").GetAsync();

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            var dtos = await response.ResponseMessage.Content.ReadFromJsonAsync<IEnumerable<ProductSaleDto>>();
            dtos.ShouldNotBeNull();
            dtos.ShouldNotBeEmpty();
            dtos.Count().ShouldBeGreaterThan(1);
        }

        [Fact]
        public async Task should_get_all_product_sales_by_order_id_and_return_200()
        {
            var order = await AddDefaultOrderAsync();

            var response = await _client.Request($"{Path}/by-order/{order.Id.Value}").GetAsync();

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            var dtos = await response.ResponseMessage.Content.ReadFromJsonAsync<IEnumerable<ProductSaleDto>>();
            dtos.ShouldNotBeNull();
            dtos.ShouldNotBeEmpty();
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

        private const string Path = "/api/product-sales";
        private readonly IProductSaleRepository _productSaleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAdditonRepository _additionRepository;
        private readonly IOrderRepository _orderRepository;

        public ProductSaleControllerTests(TestApplicationFactory<Program> factory) : base(factory)
        {
            _productSaleRepository = GetRequiredService<IProductSaleRepository>();
            _productRepository = GetRequiredService<IProductRepository>();
            _additionRepository = GetRequiredService<IAdditonRepository>();
            _orderRepository = GetRequiredService<IOrderRepository>();
        }
    }
}
