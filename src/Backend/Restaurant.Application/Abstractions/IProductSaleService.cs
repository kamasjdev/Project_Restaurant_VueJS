using Restaurant.Application.DTO;

namespace Restaurant.Application.Abstractions
{
    public interface IProductSaleService
    {
        Task<Guid> AddAsync(ProductSaleDto productSaleDto);
        Task UpdateAsync(ProductSaleDto productSaleDto);
        Task<IEnumerable<ProductSaleDto>> GetAllByOrderIdAsync(Guid orderId);
    }
}
