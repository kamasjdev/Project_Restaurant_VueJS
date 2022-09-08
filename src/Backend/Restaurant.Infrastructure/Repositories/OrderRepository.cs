using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;

namespace Restaurant.Infrastructure.Repositories
{
    internal sealed class OrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders = new();

        public async Task AddAsync(Order order)
        {
            await Task.CompletedTask;
            _orders.Add(order);
        }

        public async Task DeleteAsync(Order order)
        {
            await Task.CompletedTask;
            _orders.Remove(order);
        }

        public async Task<Order> GetAsync(Guid id)
        {
            await Task.CompletedTask;
            return _orders.SingleOrDefault(a => a.Id == id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            await Task.CompletedTask;
            return _orders;
        }

        public Task UpdateAsync(Order order)
        {
            return Task.CompletedTask;
        }
    }
}
