
using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Services
{
    public interface IAuthService
    {
        Task<RegisterUserResult> RegisterAsync(RegisterDto dto);
    }
}
