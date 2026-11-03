using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Services
{
    public record LoginUserResult(LoginUserStatus Status, AuthResponseDto? AuthResponse);
    
}
