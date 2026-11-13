namespace SafetyVault.Api.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<ChecklistItem> ChecklistItems { get; set; } = new List<ChecklistItem>();
        public ICollection<StockBatch> StockBatches { get; set; } = new List<StockBatch>();
        public User User { get; set; } = null!;
    }
}
