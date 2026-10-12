using SafetyVault.Shared.Models;

namespace SafetyVault.Shared.Rules
{
    public static class ExpiryRules
    {
        private const int UrgentDays = 30;
        private const int SoonDays = 90;

        public static int? GetDaysUntilExpiry(DateOnly? bestBefore, DateOnly today)
        {
            if (bestBefore is null)
            {
                return null;
            }
            return bestBefore.Value.DayNumber - today.DayNumber;
        }

        public static ExpiryStatus GetExpiryStatus(DateOnly? bestBefore, DateOnly today)
        {
            var daysUntilExpiry = GetDaysUntilExpiry(bestBefore, today);
            if (daysUntilExpiry is null)
            {
                return ExpiryStatus.None;
            }
            else if (daysUntilExpiry < 0)
            {
                return ExpiryStatus.Expired;
            }
            else if (daysUntilExpiry <= UrgentDays)
            {
                return ExpiryStatus.Urgent;
            }
            else if (daysUntilExpiry <= SoonDays)
            {
                return ExpiryStatus.Soon;
            }
            else
            {
                return ExpiryStatus.Ok;
            }
        }
    }
}
