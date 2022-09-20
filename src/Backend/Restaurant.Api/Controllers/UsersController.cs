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
        public async Task<ActionResult> ChangeEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(User.Identity?.Name))
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(User.Identity?.Name);
            await _userService.ChangeEmailAsync(new ChangeEmailDto(userId, email));
            return Ok();
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePasswordAsync(string password, string newPassword, string newPasswordConfirm)
        {
            if (string.IsNullOrWhiteSpace(User.Identity?.Name))
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(User.Identity?.Name);
            await _userService.ChangePasswordAsync(new ChangePasswordDto(userId, password, newPassword, newPasswordConfirm));
            return Ok();
        }

        [Authorize(Policy = "is-admin")]
        [HttpPatch("change-role/{userId:guid}")]
        public async Task<ActionResult> ChangeRoleAsync(Guid userId, string newRole)
        {
            await _userService.UpdateRoleAsync(new UpdateRoleDto(userId, newRole));
            return Ok();
        }
    }
}
