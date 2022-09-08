using NHibernate;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Infrastructure.Mappings;

namespace Restaurant.Infrastructure.Repositories
{
    internal sealed class AdditionRepository : IAdditonRepository
    {
        private readonly List<Addition> _additions = new();
        private readonly ISession _session;

        public AdditionRepository(ISession session)
        {
            _session = session;
        }

        public async Task AddAsync(Addition addition)
        {
            await Task.CompletedTask;
            //_additions.Add(addition);
            await _session.SaveAsync(addition.AsPoco());
            await _session.FlushAsync();
        }

        public async Task DeleteAsync(Addition addition)
        {
            await Task.CompletedTask;
            _additions.Remove(addition);
        }

        public async Task<Addition> GetAsync(Guid id)
        {
            await Task.CompletedTask;
            //return _additions.SingleOrDefault(a => a.Id == id);
            return (await _session.GetAsync<AdditionPoco>(id)).AsEntity();
        }

        public async Task<IEnumerable<Addition>> GetAllAsync()
        {
            await Task.CompletedTask;
            return _additions;
        }

        public Task UpdateAsync(Addition addition)
        {
            return Task.CompletedTask;
        }
    }
}
