using Restaurant.Application.DTO;

namespace Restaurant.Application.Abstractions
{
    public interface IOrderService
    {
        Task<OrderDetailsDto> GetAsync(Guid id);
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task<Guid> AddAsync(OrderDto order);
        Task<Guid> AddAsync(OrderDetailsDto orderDetailsDto);
        Task UpdateAsync(OrderDto order);
        Task DeleteAsync(Guid id);
        Task DeleteWithPositionsAsync(IEnumerable<Guid> ids);
    }
}
