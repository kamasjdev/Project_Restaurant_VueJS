using Flurl.Http;
using Restaurant.Application.DTO;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
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
    public class ProductControllerTests : BaseIntegrationTest
    {
        [Fact]
        public async Task should_add_product_and_return_201_with_id()
        {
            var dto = new ProductDto { ProductName = "Product#1", Price = 50, ProductKind = ProductKind.Soup.ToString() };

            var response = await _client.Request(Path).PostJsonAsync(dto);

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe((int)HttpStatusCode.Created);
            var id = GetIdFromHeader(response, Path);
            id.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public async Task should_add_product_and_return_from_db()
        {
            var dto = new ProductDto { ProductName = "Product#1", Price = 20, ProductKind = ProductKind.Pizza.ToString() };

            var response = await _client.Request(Path).PostJsonAsync(dto);

            response.ShouldNotBeNull();
            var id = GetIdFromHeader(response, Path);
            var productAdded = await _client.Request($"{Path}/{id}").GetJsonAsync<ProductDto>();
            productAdded.ShouldNotBeNull();
            productAdded.Id.ShouldBe(id);
            productAdded.Price.ShouldBe(dto.Price);
            productAdded.ProductName.ShouldBe(dto.ProductName);
            productAdded.ProductKind.ShouldBe(dto.ProductKind);
        }

        [Fact]
        public async Task should_update_product_and_return_staus_code_204()
        {
            var product = await AddDefaultProduct();
            var dto = new ProductDto { ProductName = "Product#1", Price = 20, ProductKind = ProductKind.MainDish.ToString() };

            var response = await _client.Request($"{Path}/{product.Id.Value}").PutJsonAsync(dto);

            response.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task should_update_product_and_return_from_db()
        {
            var product = await AddDefaultProduct();
            var dto = new ProductDto { ProductName = "Product#1", Price = 20, ProductKind = ProductKind.MainDish.ToString() };

            await _client.Request($"{Path}/{product.Id.Value}").PutJsonAsync(dto);

            var productUpdated = await _client.Request($"{Path}/{product.Id.Value}").GetJsonAsync<ProductDto>();
            productUpdated.ShouldNotBeNull();
            productUpdated.Id.ShouldBe(product.Id.Value);
            productUpdated.Price.ShouldBe(dto.Price);
            productUpdated.ProductName.ShouldBe(dto.ProductName);
            productUpdated.ProductKind.ShouldBe(dto.ProductKind);
        }

        [Fact]
        public async Task should_delete_product_and_return_200()
        {
            var product = await AddDefaultProduct();
            
            await _client.Request($"{Path}/{product.Id.Value}").DeleteAsync();

            var productDeleted = await _productRepository.GetAsync(product.Id.Value);
            productDeleted.ShouldBeNull();
        }

        [Fact]
        public async Task should_get_product_and_return_200()
        {
            var product = await AddDefaultProduct();

            var response = await _client.Request($"{Path}/{product.Id.Value}").GetAsync();
            var dto = await response.ResponseMessage.Content.ReadFromJsonAsync<ProductDto>();

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(product.Id.Value);
            dto.Price.ShouldBe(product.Price.Value);
            dto.ProductName.ShouldBe(product.ProductName.Value);
            dto.ProductKind.ShouldBe(product.ProductKind.ToString());
        }

        [Fact]
        public async Task should_get_all_products()
        {
            await AddDefaultProduct();
            await AddDefaultProduct();
            await AddDefaultProduct();

            var response = await _client.Request(Path).GetAsync();
            var dtos = await response.ResponseMessage.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            dtos.ShouldNotBeNull();
            dtos.ShouldNotBeEmpty();
            dtos.Count().ShouldBeGreaterThan(0);
        }

        private async Task<Product> AddDefaultProduct()
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

        private const string Path = "/api/products";
        private IProductRepository _productRepository;

        public ProductControllerTests(TestApplicationFactory<Program> factory) : base(factory)
        {
            _productRepository = GetRequiredService<IProductRepository>();
        }
    }
}
