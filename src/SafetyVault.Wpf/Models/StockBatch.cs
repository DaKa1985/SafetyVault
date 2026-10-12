using SafetyVault.Shared.Models;

namespace SafetyVault.Wpf.Models
{
    public class StockBatch
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateOnly? BestBefore { get; set; }
        public int? DaysUntilExpiry { get; set; }
        public ExpiryStatus ExpiryStatus { get; set; }
    }
}
