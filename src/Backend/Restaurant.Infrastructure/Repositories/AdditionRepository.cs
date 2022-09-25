using NHibernate;
using NHibernate.Linq;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;

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

        public async Task DeleteAsync(Guid id)
        {
            var query = _session.CreateQuery("DELETE FROM Addition WHERE Id = :additionId"); // HQL Query Table Additions but Entity Addition
            query.SetParameter("additionId", id);
            await query.ExecuteUpdateAsync();
            await _session.FlushAsync();
        }

        public async Task DeleteAsync(Addition addition)
        {
            await _session.DeleteAsync(addition);
            await _session.FlushAsync();
        }

        public async Task<Addition> GetAsync(Guid id)
        {
            return await _session.Query<Addition>()
                .Where(a => a.Id == id)
                .Select(a => new Addition(a.Id, a.AdditionName, a.Price, a.AdditionKind, a.ProductSales))
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Addition>> GetAllAsync()
        {
            return await _session.Query<Addition>()
                .Select(a => new Addition(a.Id, a.AdditionName, a.Price, a.AdditionKind, null))
                .ToListAsync();
        }

        public async Task UpdateAsync(Addition addition)
        {
            await _session.UpdateAsync(addition);
            await _session.FlushAsync();
        }
    }
}
