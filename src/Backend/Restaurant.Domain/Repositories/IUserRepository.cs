using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<User> GetAsync(Guid id);
        Task<User> GetAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
    }
}
