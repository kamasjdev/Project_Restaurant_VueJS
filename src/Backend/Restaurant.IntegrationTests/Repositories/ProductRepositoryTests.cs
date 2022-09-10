using Microsoft.Extensions.DependencyInjection;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.IntegrationTests.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Restaurant.IntegrationTests.Repositories
{
    public class ProductRepositoryTests : BaseIntegrationTest
    {
        [Fact]
        public async Task should_add_product()
        {
            var product = new Product(Guid.NewGuid(), "Product#1", 100, ProductKind.MainDish);

            await _productRepository.AddAsync(product);

            var productAdded = await _productRepository.GetAsync(product.Id);
            Assert.NotNull(productAdded);
            Assert.Equal(product.ProductName.Value, productAdded.ProductName.Value);
            Assert.Equal(product.ProductKind, productAdded.ProductKind);
            Assert.Equal(product.Price.Value, productAdded.Price.Value);
        }

        [Fact]
        public async Task should_update_product()
        {
            var product = await AddDefaultProductAsync();
            product.ChangeProductName("Product#2022");
            product.ChangePrice(2000);
            product.ChangeProductKind(ProductKind.Pizza);

            await _productRepository.UpdateAsync(product);

            var productUpdated = await _productRepository.GetAsync(product.Id);
            Assert.NotNull(productUpdated);
            Assert.Equal(product.ProductName.Value, productUpdated.ProductName.Value);
            Assert.Equal(product.ProductKind, productUpdated.ProductKind);
            Assert.Equal(product.Price.Value, productUpdated.Price.Value);
        }

        [Fact]
        public async Task should_delete_product()
        {
            var product = await AddDefaultProductAsync();

            await _productRepository.DeleteAsync(product);

            var productDeleted = await _productRepository.GetAsync(product.Id);
            Assert.Null(productDeleted);
        }

        [Fact]
        public async Task should_get_all_products()
        {
            await AddDefaultProductAsync();
            await AddDefaultProductAsync();

            var orders = await _productRepository.GetAllAsync();

            Assert.NotNull(orders);
            Assert.NotEmpty(orders);
            Assert.True(orders.Count() > 0);
        }

        private async Task<Product> AddDefaultProductAsync()
        {
            var product = new Product(Guid.NewGuid(), $"Product-{Guid.NewGuid()}", 100, ProductKind.MainDish);
            await _productRepository.AddAsync(product);
            return product;
        }

        private readonly IProductRepository _productRepository;

        public ProductRepositoryTests(TestApplicationFactory<Program> factory)
        {
            _productRepository = factory.Services.GetRequiredService<IProductRepository>();
        }
    }

    public static class AssertAsync
    {
        public static void CompletesIn(int timeout, Action action)
        {
            var task = Task.Run(action);
            var completedInTime = Task.WaitAll(new[] { task }, TimeSpan.FromSeconds(timeout));

            if (task.Exception != null)
            {
                if (task.Exception.InnerExceptions.Count == 1)
                {
                    throw task.Exception.InnerExceptions[0];
                }

                throw task.Exception;
            }

            if (!completedInTime)
            {
                throw new TimeoutException($"Task did not complete in {timeout} seconds.");
            }
        }
    }
}
