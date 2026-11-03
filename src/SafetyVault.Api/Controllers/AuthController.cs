using Microsoft.AspNetCore.Mvc;
using SafetyVault.Api.Services;
using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService
                ?? throw new ArgumentNullException(nameof(authService));
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            switch (result.Status)
            {
                case RegisterUserStatus.Created:
                    return StatusCode(StatusCodes.Status201Created, result.User);
                case RegisterUserStatus.UserNameAlreadyExists:
                    return Conflict("Username already exists.");
                case RegisterUserStatus.UserEmailAlreadyExists:
                    return Conflict("Email already exists.");
                default:
                    throw new InvalidOperationException($"Unexpected status: {result.Status}");
            }
        }
    }
}
