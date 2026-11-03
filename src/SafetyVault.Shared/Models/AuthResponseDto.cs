

namespace SafetyVault.Shared.Models
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTimeOffset ExpiresAt { get; set; }
    }
}
