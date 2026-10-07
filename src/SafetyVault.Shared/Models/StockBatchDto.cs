namespace SafetyVault.Shared.Models
{
    public class StockBatchDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateOnly? BestBefore { get; set; }
    }
}
