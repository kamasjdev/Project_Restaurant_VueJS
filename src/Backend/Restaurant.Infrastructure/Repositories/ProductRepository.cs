using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;

namespace Restaurant.Infrastructure.Repositories
{
    internal sealed class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new();

        public async Task AddAsync(Product product)
        {
            await Task.CompletedTask;
            _products.Add(product);
        }

        public async Task DeleteAsync(Product product)
        {
            await Task.CompletedTask;
            _products.Remove(product);
        }

        public async Task<Product> GetAsync(Guid id)
        {
            await Task.CompletedTask;
            return _products.SingleOrDefault(a => a.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            await Task.CompletedTask;
            return _products;
        }

        public Task UpdateAsync(Product product)
        {
            return Task.CompletedTask;
        }
    }
}
