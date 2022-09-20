using NHibernate;
using NHibernate.Linq;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Infrastructure.Mappings;

namespace Restaurant.Infrastructure.Repositories
{
    internal sealed class UserRepository : IUserRepository
    {
        private readonly ISession _session;

        public UserRepository(ISession session)
        {
            _session = session;
        }

        public async Task AddAsync(User user)
        {
            await _session.SaveAsync(user.AsPoco());
            await _session.FlushAsync();
        }

        public async Task DeleteAsync(User user)
        {
            await _session.DeleteAsync(_session.Load<UserPoco>(user.Id.Value));
            await _session.FlushAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _session.Query<UserPoco>()
                .Select(u => new UserPoco
                {
                    Id = u.Id,
                    CreatedAt = u.CreatedAt,
                    Role = u.Role,
                    Email = u.Email,
                    Password = u.Password
                }.AsEntity())
                .ToListAsync();
        }

        public async Task<User> GetAsync(Guid id)
        {
            return (await _session.Query<UserPoco>()
                   .Where(u => u.Id == id)
                   .Select(u => new UserPoco
                   {
                       Id = u.Id,
                       CreatedAt = u.CreatedAt,
                       Role = u.Role,
                       Email = u.Email,
                       Password = u.Password
                   }).SingleOrDefaultAsync())?.AsEntity();
        }

        public async Task<User> GetAsync(string email)
        {
            return (await _session.Query<UserPoco>()
                   .Where(u => u.Email == email)
                   .Select(u => new UserPoco
                   {
                       Id = u.Id,
                       CreatedAt = u.CreatedAt,
                       Role = u.Role,
                       Email = u.Email,
                       Password = u.Password
                   }).SingleOrDefaultAsync())?.AsEntity();
        }

        public async Task UpdateAsync(User user)
        {
            await _session.MergeAsync(user.AsPoco());
            await _session.FlushAsync();
        }
    }
}
