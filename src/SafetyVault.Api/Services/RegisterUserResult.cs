
using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Services
{
    public record RegisterUserResult(RegisterUserStatus Status, UserDto? User);
}
