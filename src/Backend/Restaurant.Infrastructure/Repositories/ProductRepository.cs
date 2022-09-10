using NHibernate;
using NHibernate.Linq;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Infrastructure.Mappings;

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
            await _session.SaveAsync(product.AsPoco());
            await _session.FlushAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            await _session.DeleteAsync(_session.Load<ProductPoco>(product.Id.Value));
            await _session.FlushAsync();
        }

        public async Task<Product> GetAsync(Guid id)
        {
            return (await _session.GetAsync<ProductPoco>(id)).AsEntity();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _session.Query<ProductPoco>()
                .Select(o => o.AsEntity()).ToListAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            await _session.MergeAsync(product.AsPoco());
            await _session.FlushAsync();
        }
    }
}
