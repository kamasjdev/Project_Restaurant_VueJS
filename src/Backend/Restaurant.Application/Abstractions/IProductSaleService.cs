using Restaurant.Application.DTO;

namespace Restaurant.Application.Abstractions
{
    public interface IProductSaleService
    {
        Task<Guid> AddAsync(AddProductSaleDto productSaleDto);
        Task UpdateAsync(AddProductSaleDto productSaleDto);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<ProductSaleDto>> GetAllByOrderIdAsync(Guid orderId);
    }
}
