using Restaurant.Application.Abstractions;
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
            var user = User.Create(Email.Of("admin@admin-test.com"), "pasW0Rd", User.Roles.AdminRole, _currentDate);

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

        [Fact]
        public async Task should_get_admin_user()
        {
            var email = "admin@admin.com";
            var password = "PasW0Rd!26";

            var user = await _userRepository.GetAsync(email);

            Assert.NotNull(user);
            Assert.Equal(email, user.Email.Value);
            Assert.True(_passwordManager.Validate(password, user.Password));
        }

        private async Task<User> AddDefaultUser()
        {
            var user = User.Create(Email.Of($"test@test.{Guid.NewGuid().ToString("N")}.com"), "pasW0Rd1", _currentDate);
            await _userRepository.AddAsync(user);
            return user;
        }

        private readonly DateTime _currentDate = DateTime.UtcNow;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordManager _passwordManager;

        public UserRepositoryTests(TestApplicationFactory<Program> factory) : base(factory)
        {
            _userRepository = GetService<IUserRepository>();
            _passwordManager = GetService<IPasswordManager>();
        }
    }
}
