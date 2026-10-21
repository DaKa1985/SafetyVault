using SafetyVault.Shared.Models;
using SafetyVault.Shared.Rules;

namespace SafetyVault.Shared.Tests
{
    public class ExpiryRulesTests
    {
        [Fact]
        public void GetExpiryStatus_NoBestBefore_ReturnsNone()
        {
            // Arrange
            var today = new DateOnly(2026, 10, 21);
            DateOnly? bestBefore = null;

            // Act
            var result = ExpiryRules.GetExpiryStatus(bestBefore, today);

            // Assert
            Assert.Equal(ExpiryStatus.None, result);
        }

        [Theory]
        [InlineData(-1, ExpiryStatus.Expired)]
        [InlineData(0, ExpiryStatus.Urgent)]
        [InlineData(30, ExpiryStatus.Urgent)]
        [InlineData(31, ExpiryStatus.Soon)]
        [InlineData(90, ExpiryStatus.Soon)]
        [InlineData(91, ExpiryStatus.Ok)]
        public void GetExpiryStatus_VariousDaysLeft_ReturnsExpectedStatus(int daysLeft, ExpiryStatus expectedStatus)
        {
            // Arrange
            var today = new DateOnly(2026, 10, 21);
            var bestBefore = today.AddDays(daysLeft);

            // Act
            var result = ExpiryRules.GetExpiryStatus(bestBefore, today);
            
            // Assert
            Assert.Equal(expectedStatus, result);
        }
    }
}