using SafetyVault.Shared.Models;
using SafetyVault.Api.Entities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;

namespace SafetyVault.Api.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly TimeProvider _timeProvider;

        public TokenService(IConfiguration configuration, TimeProvider timeProvider)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
        }

        public AuthResponseDto GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var issuer = jwtSettings["Issuer"]
                ?? throw new InvalidOperationException("Konfiguration 'Jwt:Issuer' fehlt.");
            var audience = jwtSettings["Audience"]
                ?? throw new InvalidOperationException("Konfiguration 'Jwt:Audience' fehlt.");
            var expiresInMinutes = int.Parse(jwtSettings["ExpiresInMinutes"] 
                ?? throw new InvalidOperationException("Konfiguration 'Jwt:ExpiresInMinutes' fehlt."));
            var jwtSigningKey = jwtSettings["SigningKey"]
                ?? throw new InvalidOperationException("Konfiguration 'Jwt:SigningKey' fehlt.");
            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtSigningKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new Dictionary<string, object>
            {
                { JwtRegisteredClaimNames.Sub, user.Id.ToString() },
                { JwtRegisteredClaimNames.Name, user.UserName },
                { JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString() }
            };
            var now = _timeProvider.GetUtcNow();
            var expiresAt = now.AddMinutes(expiresInMinutes);
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Audience = audience,
                Claims = claims,
                IssuedAt = now.UtcDateTime,
                Expires = expiresAt.UtcDateTime,
                SigningCredentials = credentials,
                NotBefore = now.UtcDateTime
            };
            var handler = new JsonWebTokenHandler();
            var token = handler.CreateToken(descriptor);

            return new AuthResponseDto
            {
                Token = token,
                ExpiresAt = expiresAt
            };
        }
    }
}
