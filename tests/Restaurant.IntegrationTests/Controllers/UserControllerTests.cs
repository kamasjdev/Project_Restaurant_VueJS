using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Restaurant.Application.Abstractions;
using Restaurant.Application.DTO;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Domain.ValueObjects;
using Restaurant.IntegrationTests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Restaurant.IntegrationTests.Controllers
{
    public class UserControllerTests : BaseIntegrationTest
    {
        [Fact]
        public async Task should_sign_up()
        {
            var dto = new SignUpDto("emailtestabc123@pol.and", "PasWord1234!2", User.Roles.UserRole);

            var response = await _client.Request($"{Path}/sign-up").PostJsonAsync(dto);
            
            response.StatusCode.ShouldBe(StatusCodes.Status201Created);
            var (responseHeaderName, responseHeaderValue) = response.Headers.Where(h => h.Name == "Location").FirstOrDefault();
            responseHeaderValue.ShouldContain("me");
            var userAdded = await _userRepository.GetAsync(dto.Email);
            userAdded.Email.Value.ShouldBe(dto.Email);
            userAdded.Role.ShouldBe(dto.Role);
        }

        [Fact]
        public async Task should_sign_in()
        {
            var user = await AddDefaultUserAsync();
            var dto = new SignInDto(user.Email.Value, StandardPassword);

            var response = await _client.Request($"{Path}/sign-in").PostJsonAsync(dto);

            response.StatusCode.ShouldBe(StatusCodes.Status200OK);
            var authDto = await response.ResponseMessage.Content.ReadFromJsonAsync<AuthDto>();
            authDto.ShouldNotBeNull();
            authDto.AccessToken.ShouldNotBeNullOrWhiteSpace();
            authDto.AccessToken.ShouldContain(".");
            authDto.Role.ShouldBe(User.Roles.UserRole);
        }

        [Fact]
        public async Task should_update_user()
        {
            var user = await AddDefaultUserAsync();
            var newEmail = "email@mail1234.com";
            var dto = new UpdateUserDto(user.Id, newEmail, StandardPassword, User.Roles.AdminRole);

            var response = await _client.Request($"{Path}/{user.Id.Value}").PutJsonAsync(dto);

            response.StatusCode.ShouldBe(StatusCodes.Status204NoContent);
            var userUpdated = await _client.Request($"{Path}/{user.Id.Value}").GetJsonAsync<UserDto>();
            userUpdated.ShouldNotBeNull();
            userUpdated.Email.ShouldBe(newEmail);
            userUpdated.Role.ShouldBe(dto.Role);
        }

        [Fact]
        public async Task should_change_role()
        {
            var user = await AddDefaultUserAsync();
            var dto = new UpdateRoleDto(user.Id, User.Roles.AdminRole);

            var response = await _client.Request($"{Path}/change-role/{user.Id.Value}").PatchJsonAsync(dto);

            response.StatusCode.ShouldBe(StatusCodes.Status200OK);
            var userUpdated = await _client.Request($"{Path}/{user.Id.Value}").GetJsonAsync<UserDto>();
            userUpdated.ShouldNotBeNull();
            userUpdated.Role.ShouldBe(dto.Role);
        }

        [Fact]
        public async Task should_get_all_users()
        {
            await AddDefaultUserAsync();
            await AddDefaultUserAsync();
            await AddDefaultUserAsync();

            var response = await _client.Request(Path).GetAsync();

            response.StatusCode.ShouldBe(StatusCodes.Status200OK);
            var dtos = await response.ResponseMessage.Content.ReadFromJsonAsync<IEnumerable<UserDto>>();
            dtos.ShouldNotBeEmpty();
            dtos.Count().ShouldBeGreaterThan(2);
        }

        [Fact]
        public async Task given_request_unauthorized_when_change_email_should_return_401()
        {
            _client.Headers.Remove("Authorization");

            var response = await _client.Request($"{Path}/change-email").AllowHttpStatus("401").PostJsonAsync(new ChangeEmailDto(Guid.NewGuid(), "email2123@abc.com"));

            response.StatusCode.ShouldBe(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task given_valid_user_should_change_email()
        {
            var user = await AddDefaultUserAsync();
            await SignIn(user.Email.Value, StandardPassword);
            var dto = new ChangeEmailDto(user.Id, "email2123xad@abc.com");

            var response = await _client.Request($"{Path}/change-email").PostJsonAsync(dto);

            response.StatusCode.ShouldBe(StatusCodes.Status200OK);
            var userUpdated = await _userRepository.GetAsync(user.Id);
            userUpdated.Email.Value.ShouldBe(dto.Email);
        }

        [Fact]
        public async Task given_valid_user_should_change_password()
        {
            var user = await AddDefaultUserAsync();
            await SignIn(user.Email.Value, StandardPassword);
            var newPassword = "Ne3WP4Sw8O0rd";
            var dto = new ChangePasswordDto(user.Id, StandardPassword, newPassword, newPassword);

            var response = await _client.Request($"{Path}/change-password").PostJsonAsync(dto);

            response.StatusCode.ShouldBe(StatusCodes.Status200OK);
            var userUpdated = await _userRepository.GetAsync(user.Id);
            userUpdated.Password.ShouldNotBe(user.Password);
        }

        [Fact]
        public async Task given_valid_user_should_change_password_and_sign_with_new_credentials()
        {
            var user = await AddDefaultUserAsync();
            await SignIn(user.Email.Value, StandardPassword);
            var newPassword = "Ne3WP4Sw8O0rd";
            var dto = new ChangePasswordDto(user.Id, StandardPassword, newPassword, newPassword);

            var response = await _client.Request($"{Path}/change-password").PostJsonAsync(dto);

            response.StatusCode.ShouldBe(StatusCodes.Status200OK);
            var dtoSignIn = new SignInDto(user.Email.Value, newPassword);

            var responseSignIn = await _client.Request($"{Path}/sign-in").PostJsonAsync(dtoSignIn);

            responseSignIn.StatusCode.ShouldBe(StatusCodes.Status200OK);
            var authDto = await responseSignIn.ResponseMessage.Content.ReadFromJsonAsync<AuthDto>();
            authDto.ShouldNotBeNull();
            authDto.AccessToken.ShouldNotBeNullOrWhiteSpace();
            authDto.AccessToken.ShouldContain(".");
            authDto.Role.ShouldBe(User.Roles.UserRole);
        }

        [Fact]
        public async Task given_unauthenticated_request_when_get_user_should_return_401()
        {
            var user = await AddDefaultUserAsync();
            _client.Headers.Remove("Authorization");

            var response = await _client.Request($"{Path}/{user.Id.Value}").AllowHttpStatus("401").GetAsync();

            response.StatusCode.ShouldBe(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task given_user_with_no_permission_when_get_user_should_return_403()
        {
            var user = await AddDefaultUserAsync();
            await SignIn(user.Email.Value, StandardPassword);

            var response = await _client.Request($"{Path}/{user.Id.Value}").AllowHttpStatus("403").GetAsync();

            response.StatusCode.ShouldBe(StatusCodes.Status403Forbidden);
        }

        [Fact]
        public async Task should_get_user_by_id_and_return_200_with_dto()
        {
            var user = await AddDefaultUserAsync();

            var response = await _client.Request($"{Path}/{user.Id.Value}").GetAsync();

            response.StatusCode.ShouldBe(StatusCodes.Status200OK);
            var userAdded = await response.ResponseMessage.Content.ReadFromJsonAsync<UserDto>();
            userAdded.ShouldNotBeNull();
            userAdded.Email.ShouldBe(user.Email.Value);
            userAdded.Role.ShouldBe(user.Role);
        }

        private async Task SignIn(string email, string password)
        {
            var dto = new SignInDto(email, password);
            var response = await _client.Request("/api/users/sign-in").PostJsonAsync(dto);
            var auth = await response.ResponseMessage.Content.ReadFromJsonAsync<AuthDto>();
            _client.WithOAuthBearerToken(auth.AccessToken);
        }
        
        public async Task<User> AddDefaultUserAsync()
        {
            var user = User.Create(Email.Of($"email{Guid.NewGuid().ToString()}@tester.com"), _passwordManager.Secure(StandardPassword), DateTime.UtcNow);
            await _userRepository.AddAsync(user);
            return user;
        }

        private const string Path = "/api/users";
        private const string StandardPassword = "StAnD4rdPassWord";
        private readonly IUserRepository _userRepository;
        private readonly IPasswordManager _passwordManager;

        public UserControllerTests(TestApplicationFactory<Program> factory) : base(factory)
        {
            _userRepository = GetRequiredService<IUserRepository>();
            _passwordManager = GetRequiredService<IPasswordManager>();
        }
    }
}
