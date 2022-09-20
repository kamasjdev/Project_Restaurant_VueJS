using Microsoft.Extensions.DependencyInjection;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Domain.ValueObjects;
using Restaurant.IntegrationTests.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Restaurant.IntegrationTests.Repositories
{
    public class UserRepositoryTests : BaseIntegrationTest
    {
        [Fact]
        public async Task should_add_user()
        {
            var user = User.Create(Email.Of("admin@admin.com"), "pasW0Rd", User.Roles.AdminRole, _currentDate);

            await _userRepository.AddAsync(user);

            var userAdded = await _userRepository.GetAsync(user.Id.Value);
            Assert.NotNull(userAdded);
            Assert.Equal(user.Email, userAdded.Email);
            Assert.Equal(user.Role, userAdded.Role);
            Assert.Equal(user.Password, userAdded.Password);
            Assert.Equal(user.CreatedAt, userAdded.CreatedAt);
        }

        [Fact]
        public async Task should_update_user()
        {
            var user = await AddDefaultUser();
            user.ChangeRole(User.Roles.AdminRole);
            user.ChangeEmail(Email.Of("email1234567@test.com"));
            user.ChangePassword("Psaw0rddt");

            await _userRepository.UpdateAsync(user);

            var userUpdated = await _userRepository.GetAsync(user.Id);
            Assert.NotNull(userUpdated);
            Assert.Equal(user.Email, userUpdated.Email);
            Assert.Equal(user.Role, userUpdated.Role);
            Assert.Equal(user.Password, userUpdated.Password);
            Assert.Equal(user.CreatedAt, userUpdated.CreatedAt);
        }

        [Fact]
        public async Task should_delete_user()
        {
            var user = await AddDefaultUser();

            await _userRepository.DeleteAsync(user);

            var userDeleted = await _userRepository.GetAsync(user.Id);
            Assert.Null(userDeleted);
        }

        [Fact]
        public async Task should_get_user_by_email()
        {
            var user = await AddDefaultUser();

            var userGetById = await _userRepository.GetAsync(user.Email.Value);

            Assert.NotNull(userGetById);
            Assert.Equal(user.Email, userGetById.Email);
            Assert.Equal(user.Role, userGetById.Role);
            Assert.Equal(user.Password, userGetById.Password);
            Assert.Equal(user.CreatedAt, userGetById.CreatedAt);
        }

        [Fact]
        public async Task should_get_all_users()
        {
            await AddDefaultUser();
            await AddDefaultUser();

            var users = await _userRepository.GetAllAsync();

            Assert.NotNull(users);
            Assert.NotEmpty(users);
            Assert.True(users.Count() > 1);
        }

        private async Task<User> AddDefaultUser()
        {
            var user = User.Create(Email.Of($"test@test.{Guid.NewGuid().ToString("N")}.com"), "pasW0Rd1", _currentDate);
            await _userRepository.AddAsync(user);
            return user;
        }

        private readonly DateTime _currentDate = DateTime.UtcNow;
        private readonly IUserRepository _userRepository;

        public UserRepositoryTests(TestApplicationFactory<Program> factory)
        {
            _userRepository = factory.Services.GetRequiredService<IUserRepository>();
        }
    }
}
