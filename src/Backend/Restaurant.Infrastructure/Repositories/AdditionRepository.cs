using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;

namespace Restaurant.Infrastructure.Repositories
{
    internal sealed class AdditionRepository : IAdditonRepository
    {
        private readonly List<Addition> _additions = new();

        public async Task AddAsync(Addition addition)
        {
            await Task.CompletedTask;
            _additions.Add(addition);
        }

        public async Task DeleteAsync(Addition addition)
        {
            await Task.CompletedTask;
            _additions.Remove(addition);
        }

        public async Task<Addition> GetAsync(Guid id)
        {
            await Task.CompletedTask;
            return _additions.SingleOrDefault(a => a.Id == id);
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
