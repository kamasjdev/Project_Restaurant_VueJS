using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Repositories
{
    public interface IProductSaleRepository
    {
        Task AddAsync(ProductSale productSale);
        Task UpdateAsync(ProductSale productSale);
        Task DeleteAsync(ProductSale productSale);
        Task<ProductSale> GetAsync(Guid id);
        Task<IEnumerable<ProductSale>> GetAllAsync();
    }
}
