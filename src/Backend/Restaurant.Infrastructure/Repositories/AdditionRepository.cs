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
            await _session.DeleteAsync(addition);
            await _session.FlushAsync();
        }

        public async Task<Addition> GetAsync(Guid id)
        {
            return await _session.Query<Addition>()
                .SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Addition>> GetAllAsync()
        {
            return await _session.Query<Addition>()
                .ToListAsync();
        }

        public async Task UpdateAsync(Addition addition)
        {
            await _session.MergeAsync(addition);
            await _session.FlushAsync();
        }
    }
}
