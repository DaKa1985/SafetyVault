using SafetyVault.Api.Entities;
using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Services
{
    public interface ITokenService
    {
        AuthResponseDto GenerateToken(User user);
    }
}
