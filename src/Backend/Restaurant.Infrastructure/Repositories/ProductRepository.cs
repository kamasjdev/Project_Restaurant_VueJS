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
            return (await _session.Query<ProductPoco>().Where(p => p.Id == id)
                .Select(p => new ProductPoco
                {
                    Id = p.Id,
                    Price = p.Price,
                    ProductKind = p.ProductKind,
                    ProductName = p.ProductName,
                    ProductSales = p.ProductSales.Select(ps => new ProductSalePoco
                    {
                        Id = ps.Id,
                        Email = ps.Email,
                        Addition = ps.Addition != null ? ps.Addition : null,
                        EndPrice = ps.EndPrice,
                        ProductSaleState = ps.ProductSaleState,
                        Order = ps.Order != null ? new OrderPoco
                        {
                            Id = ps.Order.Id,
                            Created = ps.Order.Created,
                            Email = ps.Order.Email,
                            Note = ps.Order.Note,
                            OrderNumber = ps.Order.OrderNumber,
                            Price = ps.Order.Price
                        } : null
                    }),
                }).SingleOrDefaultAsync())?.AsEntity();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _session.Query<ProductPoco>()
                 .Select(p => new ProductPoco
                 {
                     Id = p.Id,
                     Price = p.Price,
                     ProductKind = p.ProductKind,
                     ProductName = p.ProductName
                 }.AsEntity()).ToListAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            await _session.MergeAsync(product.AsPoco());
            await _session.FlushAsync();
        }
    }
}
