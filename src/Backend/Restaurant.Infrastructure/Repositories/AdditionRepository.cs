using NHibernate;
using NHibernate.Linq;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Infrastructure.Mappings;

namespace Restaurant.Infrastructure.Repositories
{
    internal sealed class AdditionRepository : IAdditonRepository
    {
        private readonly ISession _session;

        public AdditionRepository(ISession session)
        {
            _session = session;
        }

        public async Task AddAsync(Addition addition)
        {
            await _session.SaveAsync(addition);
            await _session.FlushAsync();
        }

        public async Task DeleteAsync(Addition addition)
        {
            await _session.DeleteAsync(_session.Load<AdditionPoco>(addition.Id.Value));
            await _session.FlushAsync();
        }

        public async Task<Addition> GetAsync(Guid id)
        {
            return (await _session.Query<AdditionPoco>().Where(a => a.Id == id)
                .Select(a => new AdditionPoco
                {
                    Id = a.Id,
                    AdditionName = a.AdditionName,
                    Price = a.Price,
                    AdditionKind = a.AdditionKind,
                    ProductSales = a.ProductSales.Select(p => new ProductSalePoco { Id = p.Id })
                }).SingleOrDefaultAsync())?.AsEntity();
        }

        public async Task<IEnumerable<Addition>> GetAllAsync()
        {
            return await _session.Query<AdditionPoco>()
                .Select(a => new AdditionPoco
                {
                    Id = a.Id,
                    AdditionName = a.AdditionName,
                    Price = a.Price,
                    AdditionKind = a.AdditionKind
                }.AsEntity())
                .ToListAsync();
        }

        public async Task UpdateAsync(Addition addition)
        {
            await _session.MergeAsync(addition.AsPoco());
            await _session.FlushAsync();
        }
    }
}
