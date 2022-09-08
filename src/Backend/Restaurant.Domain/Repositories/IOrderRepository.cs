using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Order order);
        Task<Order> GetAsync(Guid id);
        Task<IEnumerable<Order>> GetAllAsync();
    }
}
