using NHibernate;
using NHibernate.Linq;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;

namespace Restaurant.Infrastructure.Repositories
{
    internal sealed class ProductRepository : IProductRepository
    {
        private readonly ISession _session;

        public ProductRepository(ISession session)
        {
            _session = session;
        }

        public async Task AddAsync(Product product)
        {
            await _session.SaveAsync(product);
            await _session.FlushAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            await _session.DeleteAsync(product);
            await _session.FlushAsync();
        }

        public async Task<Product> GetAsync(Guid id)
        {
            return (await _session.Query<Product>()
                .Where(p => p.Id == id)
                .Select(p => new Product(p.Id, p.ProductName, p.Price, p.ProductKind,
                    p.Orders.Select(o => new Order(o.Id, o.OrderNumber, o.Created, o.Price, o.Email, o.Note, null))))
                .SingleOrDefaultAsync());
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _session.Query<Product>()
                .Select(p => new Product(p.Id, p.ProductName, p.Price, p.ProductKind, null))
                .ToListAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            await _session.UpdateAsync(product);
            await _session.FlushAsync();
        }

        public async Task<bool> ExistsProductSalesAsync(Guid id)
        {
            var query = _session.CreateSQLQuery(
                @"SELECT DISTINCT 1 FROM ProductSales productSale WHERE productSale.ProductId=:productId");
            query.SetParameter("productId", id);
            var result = await query.UniqueResultAsync<Int64>();
            return result == 1;
        }
    }
}
