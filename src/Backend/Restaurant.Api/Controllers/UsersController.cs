using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Abstractions;
using Restaurant.Application.DTO;

namespace Restaurant.Api.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Policy = "is-admin")]
        [HttpGet]
        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            return await _userService.GetAllAsync();
        }

        [Authorize(Policy = "is-admin")]
        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<UserDto>> GetAsync(Guid userId)
        {
            return OkOrNotFound(await _userService.GetAsync(userId));
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> Get()
        {
            if (string.IsNullOrWhiteSpace(User.Identity?.Name))
            {
                return NotFound();
            }

            var userId = Guid.Parse(User.Identity?.Name);
            return await _userService.GetAsync(userId);
        }

        [HttpPost("sign-in")]
        public async Task<ActionResult<AuthDto>> SignInAsync(SignInDto signInDto)
        {
            return Ok(await _userService.SignInAsync(signInDto));
        }

        [HttpPost("sign-up")]
        public async Task<ActionResult> SignUpAsync(SignUpDto signUpDto)
        {
            await _userService.SignUpAsync(signUpDto);
            return CreatedAtAction(nameof(Get), null, null);
        }

        [Authorize]
        [HttpPost("change-email")]
        public async Task<ActionResult> ChangeEmailAsync(ChangeEmailDto changeEmailDto)
        {
            if (string.IsNullOrWhiteSpace(User.Identity?.Name))
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(User.Identity?.Name);
            await _userService.ChangeEmailAsync(new ChangeEmailDto(userId, changeEmailDto.Email));
            return Ok();
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            if (string.IsNullOrWhiteSpace(User.Identity?.Name))
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(User.Identity?.Name);
            await _userService.ChangePasswordAsync(new ChangePasswordDto(userId, changePasswordDto.Password, changePasswordDto.NewPassword, changePasswordDto.NewPasswordConfirm));
            return Ok();
        }

        [Authorize(Policy = "is-admin")]
        [HttpPatch("change-role/{userId:guid}")]
        public async Task<ActionResult> ChangeRoleAsync(Guid userId, UpdateRoleDto updateRoleDto)
        {
            await _userService.UpdateRoleAsync(updateRoleDto with { UserId = userId});
            return Ok();
        }

        [Authorize(Policy = "is-admin")]
        [HttpPut("{userId}")]
        public async Task<ActionResult> UpdateAsync(Guid userId, UpdateUserDto updateUserDto)
        {
            await _userService.UpdateAsync(updateUserDto with { UserId = userId });
            return NoContent();
        }
    }
}
