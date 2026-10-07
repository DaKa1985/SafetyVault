namespace SafetyVault.Api.Entities
{
    public class StockBatch
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public DateOnly? BestBefore { get; set; }
        public Item Item { get; set; } = null!;
    }
}
