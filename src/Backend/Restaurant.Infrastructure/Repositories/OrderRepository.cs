using NHibernate;
using NHibernate.Linq;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
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
            var orderPoco = order.AsPoco();
            await _session.SaveAsync(orderPoco);
            await _session.FlushAsync();
        }

        public async Task DeleteAsync(Order order)
        {
            await _session.DeleteAsync(_session.Load<OrderPoco>(order.Id.Value));
            await _session.FlushAsync();
        }

        public async Task<Order> GetAsync(Guid id)
        {
            var order = await _session.Query<OrderPoco>().Where(o => o.Id == id)
                .Select(o => new OrderPoco
                {
                    Id = o.Id,
                    Email = o.Email,
                    Created = o.Created,
                    Note = o.Note,
                    OrderNumber = o.OrderNumber,
                    Price = o.Price,
                    Products = o.Products.Select(ps => new ProductSalePoco
                    {
                        Id = ps.Id,
                        Addition = new AdditionPoco
                        {
                            Id = ps.Addition.Id,
                            Price = ps.Addition.Price,
                            AdditionKind = ps.Addition.AdditionKind,
                            AdditionName = ps.Addition.AdditionName
                        },
                        Email = ps.Email,
                        EndPrice = ps.EndPrice,
                        Product = new ProductPoco
                        {
                            Id = ps.Product.Id,
                            Price = ps.Product.Price,
                            ProductKind = ps.Product.ProductKind,
                            ProductName = ps.Product.ProductName
                        },
                        ProductSaleState = ps.ProductSaleState
                    })
                }).SingleOrDefaultAsync();
            return order?.AsEntity();
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
