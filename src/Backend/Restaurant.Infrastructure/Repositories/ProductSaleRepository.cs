using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;

namespace Restaurant.Infrastructure.Repositories
{
    internal sealed class ProductSaleRepository : IProductSaleRepository
    {
        private readonly List<ProductSale> _productSales = new();

        public async Task AddAsync(ProductSale productSale)
        {
            await Task.CompletedTask;
            _productSales.Add(productSale);
        }

        public async Task DeleteAsync(ProductSale productSale)
        {
            await Task.CompletedTask;
            _productSales.Remove(productSale);
        }

        public async Task<ProductSale> GetAsync(Guid id)
        {
            await Task.CompletedTask;
            return _productSales.SingleOrDefault(a => a.Id == id);
        }

        public async Task<IEnumerable<ProductSale>> GetAllAsync()
        {
            await Task.CompletedTask;
            return _productSales;
        }

        public Task UpdateAsync(ProductSale productSale)
        {
            return Task.CompletedTask;
        }
    }
}
