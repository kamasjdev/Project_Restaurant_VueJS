using Restaurant.Application.Abstractions;
using Restaurant.Application.DTO;
using Restaurant.Application.Exceptions;
using Restaurant.Application.Mappings;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Domain.ValueObjects;

namespace Restaurant.Application.Services
{
    internal sealed class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordManager _passwordManager;
        private readonly IJwtManager _jwtManager;
        private readonly IClock _clock;

        public UserService(IUserRepository userRepository, IPasswordManager passwordManager, IJwtManager jwtManager, IClock clock)
        {
            _userRepository = userRepository;
            _passwordManager = passwordManager;
            _jwtManager = jwtManager;
            _clock = clock;
        }

        public async Task ChangeEmailAsync(ChangeEmailDto changeEmailDto)
        {
            var user = await _userRepository.GetAsync(changeEmailDto.UserId);

            if (user is null)
            {
                throw new UserNotFoundException(changeEmailDto.UserId);
            }

            user.ChangeEmail(Email.Of(changeEmailDto.Email));
            await _userRepository.UpdateAsync(user);
        }

        public async Task ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            var user = await _userRepository.GetAsync(changePasswordDto.UserId);

            if (user is null)
            {
                throw new UserNotFoundException(changePasswordDto.UserId);
            }

            var password = new Password(changePasswordDto.Password);
            var newPassword = new Password(changePasswordDto.NewPassword);
            var newPasswordConfirm = new Password(changePasswordDto.NewPasswordConfirm);

            if (newPassword != newPasswordConfirm)
            {
                throw new NewPasswordsAreNotSameException();
            }

            if (!_passwordManager.Validate(password.Value, user.Password))
            {
                throw new InvalidCredentialsException();
            }

            if (password == newPassword)
            {
                return;
            }

            user.ChangePassword(changePasswordDto.NewPassword);
            await _userRepository.UpdateAsync(user);
        }

        public async Task<UserDto> GetAsync(Guid id)
        {
            return (await _userRepository.GetAsync(id)).AsDto();
        }

        public async Task<UserDto> GetAsync(string email)
        {
            return (await _userRepository.GetAsync(email)).AsDto();
        }

        public async Task<AuthDto> SignInAsync(SignInDto signInDto)
        {
            var user = await _userRepository.GetAsync(signInDto.Email);
            if (user is null)
            {
                throw new InvalidCredentialsException();
            }

            if (!_passwordManager.Validate(signInDto.Password, user.Password))
            {
                throw new InvalidCredentialsException();
            }

            var token = _jwtManager.CreateToken(user.Id, user.Role);
            
            return new AuthDto
            {
                AccessToken = token,
                Role = user.Role
            };
        }

        public async Task SignUpAsync(SignUpDto signUpDto)
        {
            var email = new Email(signUpDto.Email);
            var password = new Password(signUpDto.Password);
            var role = string.IsNullOrWhiteSpace(signUpDto.Role) ? User.Roles.UserRole : signUpDto.Role;

            if (await _userRepository.GetAsync(email.Value) is not null)
            {
                throw new EmailAlreadyInUseException(email.Value);
            }

            var securedPassword = _passwordManager.Secure(password);
            var user = User.Create(email, securedPassword, role, _clock.CurrentDate());
            await _userRepository.AddAsync(user);
        }

        public async Task UpdateRoleAsync(UpdateRoleDto updateRoleDto)
        {
            var user = await _userRepository.GetAsync(updateRoleDto.UserId);

            if (user is null)
            {
                throw new UserNotFoundException(updateRoleDto.UserId);
            }

            user.ChangeRole(user.Role);
            await _userRepository.UpdateAsync(user);
        }
    }
}
