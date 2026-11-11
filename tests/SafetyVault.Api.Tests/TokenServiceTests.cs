using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Time.Testing;
using SafetyVault.Api.Entities;
using SafetyVault.Api.Services;

namespace SafetyVault.Api.Tests
{
    public class TokenServiceTests
    {
        [Fact]
        public void GenerateToken_ValidUser_ExpiresAfterConfiguredMinutes()
        {
            // Arrange
            var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(2026, 11, 11, 10, 0, 0, TimeSpan.Zero));
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Issuer"] = "SafetyVault.Api",
                    ["Jwt:Audience"] = "SafetyVault.Clients",
                    ["Jwt:ExpiresInMinutes"] = "60",
                    ["Jwt:SigningKey"] = Convert.ToBase64String(new byte[64])
                })
                .Build();
            var tokenService = new TokenService(configuration, fakeTimeProvider);
            var user = new User { Id = 1, UserName = "testuser" };

            // Act
            var result = tokenService.GenerateToken(user);

            // Assert
            var expected = new DateTimeOffset(2026, 11, 11, 11, 0, 0, TimeSpan.Zero);
            Assert.Equal(expected, result.ExpiresAt);
        }
    }
}
