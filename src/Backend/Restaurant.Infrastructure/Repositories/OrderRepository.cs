using NHibernate;
using NHibernate.Linq;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Domain.ValueObjects;
using Restaurant.Infrastructure.Mappings;

namespace Restaurant.Infrastructure.Repositories
{
    internal sealed class OrderRepository : IOrderRepository
    {
        private readonly ISession _session;

        public OrderRepository(ISession session)
        {
            _session = session;
        }

        public async Task AddAsync(Order order)
        {
            await _session.SaveAsync(order.AsPoco());
            await _session.FlushAsync();
        }

        public async Task DeleteAsync(Order order)
        {
            await _session.DeleteAsync(_session.Load<OrderPoco>(order.Id.Value));
            await _session.FlushAsync();
        }

        public async Task<Order> GetAsync(Guid id)
        {
            return (await _session.Query<OrderPoco>().Where(o => o.Id == id)
                .Select(o => new OrderPoco
                {
                    Id = o.Id,
                    Email = o.Email,
                    Created = o.Created,
                    Note = o.Note,
                    OrderNumber = o.OrderNumber,
                    Price = o.Price,
                    Products = o.Products.ToList()
                }).SingleOrDefaultAsync())?.AsEntity();
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _session.Query<OrderPoco>()
                .Select(o => new OrderPoco
                {
                    Id = o.Id,
                    Email = o.Email,
                    Created = o.Created,
                    Note = o.Note,
                    OrderNumber = o.OrderNumber,
                    Price = o.Price
                }.AsEntity())
                .ToListAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            await _session.MergeAsync(order.AsPoco());
            await _session.FlushAsync();
        }
    }
}
