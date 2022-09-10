using Restaurant.Application.DTO;

namespace Restaurant.Application.Abstractions
{
    public interface IProductService
    {
        Task<ProductDetailsDto> GetAsync(Guid id);
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<Guid> AddAsync(ProductDto productDto);
        Task UpdateAsync(ProductDto productDto);
        Task DeleteAsync(Guid id);
    }
}
