using NHibernate;
using NHibernate.Linq;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Infrastructure.Mappings;

namespace Restaurant.Infrastructure.Repositories
{
    internal sealed class ProductSaleRepository : IProductSaleRepository
    {
        private readonly ISession _session;

        public ProductSaleRepository(ISession session)
        {
            _session = session;
        }

        public async Task AddAsync(ProductSale productSale)
        {
            await _session.SaveAsync(productSale);
            await _session.FlushAsync();
        }

        public async Task DeleteAsync(ProductSale productSale)
        {
            await _session.DeleteAsync(productSale);
            await _session.FlushAsync();
        }

        public async Task<ProductSale> GetAsync(Guid id)
        {
            return (await _session.Query<ProductSale>().Where(p => p.Id == id)
                .Select(p => new ProductSale(p.Id, 
                    new Product(p.Product.Id, p.Product.ProductName, p.Product.Price, p.Product.ProductKind, null),
                    p.ProductSaleState, p.Email, 
                    p.Addition != null ? new Addition(p.Addition.Id, p.Addition.AdditionName, p.Addition.Price, p.Addition.AdditionKind, null) : null,
                    p.Order != null ? new Order(p.Order.Id, p.Order.OrderNumber, p.Order.Created, p.Order.Price, p.Order.Email, p.Order.Note, null) : null))
                .SingleOrDefaultAsync());
        }

        public async Task<IEnumerable<ProductSale>> GetAllAsync()
        {
            return await _session.Query<ProductSale>()
                .ToListAsync();
        }

        public async Task UpdateAsync(ProductSale productSale)
        {
            await _session.UpdateAsync(productSale);
            await _session.FlushAsync();
        }

        public async Task<IEnumerable<ProductSale>> GetAllByOrderIdAsync(Guid orderId)
        {
            return await _session.Query<ProductSale>()
                .Where(p => p.Order.Id == orderId)
                .Select(p => new ProductSale(p.Id,
                    new Product(p.Product.Id, p.Product.ProductName, p.Product.Price, p.Product.ProductKind, null),
                    p.ProductSaleState, p.Email,
                    p.Addition != null ? new Addition(p.Addition.Id, p.Addition.AdditionName, p.Addition.Price, p.Addition.AdditionKind, null) : null,
                    p.Order != null ? new Order(p.Order.Id, p.Order.OrderNumber, p.Order.Created, p.Order.Price, p.Order.Email, p.Order.Note, null) : null))
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductSale>> GetAllByOrderIdAsync(string email)
        {
            return await _session.Query<ProductSale>()
                .Where(p => p.Email.Value == email)
                .Select(p => new ProductSale(p.Id,
                    new Product(p.Product.Id, p.Product.ProductName, p.Product.Price, p.Product.ProductKind, null),
                    p.ProductSaleState, p.Email,
                    p.Addition != null ? new Addition(p.Addition.Id, p.Addition.AdditionName, p.Addition.Price, p.Addition.AdditionKind, null) : null,
                    p.Order != null ? new Order(p.Order.Id, p.Order.OrderNumber, p.Order.Created, p.Order.Price, p.Order.Email, p.Order.Note, null) : null))
                .ToListAsync();
        }
    }
}
