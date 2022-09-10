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
            await _session.SaveAsync(productSale.AsPoco());
            await _session.FlushAsync();
        }

        public async Task DeleteAsync(ProductSale productSale)
        {
            await _session.DeleteAsync(_session.Load<ProductSalePoco>(productSale.Id.Value));
            await _session.FlushAsync();
        }

        public async Task<ProductSale> GetAsync(Guid id)
        {
            return (await _session.Query<ProductSalePoco>().Where(p => p.Id == id)
                .Select(p => new ProductSalePoco
                {
                    Id = p.Id,
                    EndPrice = p.EndPrice,
                    ProductSaleState = p.ProductSaleState,
                    Email = p.Email,
                    Product = new ProductPoco { Id = p.Product.Id, Price = p.Product.Price, ProductKind = p.Product.ProductKind, ProductName = p.Product.ProductName },
                    Addition = p.Addition != null ? new AdditionPoco { Id = p.Addition.Id, AdditionKind = p.Addition.AdditionKind, AdditionName = p.Addition.AdditionName, Price = p.Addition.Price } : null,
                    Order = p.Order != null ? new OrderPoco { Id = p.Order.Id, OrderNumber = p.Order.OrderNumber, Created = p.Order.Created, Email = p.Order.Email, Note = p.Order.Note, Price = p.Order.Price } : null
                }).SingleOrDefaultAsync())?.AsEntity();
        }

        public async Task<IEnumerable<ProductSale>> GetAllAsync()
        {
            return await _session.Query<ProductSalePoco>()
                .Select(p => new ProductSalePoco
                {
                    Id = p.Id,
                    EndPrice = p.EndPrice,
                    ProductSaleState = p.ProductSaleState,
                    Email = p.Email,
                    Product = new ProductPoco { Id = p.Product.Id, Price = p.Product.Price, ProductKind = p.Product.ProductKind, ProductName = p.Product.ProductName },
                    Addition = p.Addition != null ? new AdditionPoco { Id = p.Addition.Id, AdditionKind = p.Addition.AdditionKind, AdditionName = p.Addition.AdditionName, Price = p.Addition.Price } : null,
                    Order = p.Order != null ? new OrderPoco { Id = p.Order.Id, OrderNumber = p.Order.OrderNumber, Created = p.Order.Created, Email = p.Order.Email, Note = p.Order.Note, Price = p.Order.Price } : null
                }.AsEntity())
                .ToListAsync();
        }

        public async Task UpdateAsync(ProductSale productSale)
        {
            await _session.MergeAsync(productSale.AsPoco());
            await _session.FlushAsync();
        }
    }
}
