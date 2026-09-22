namespace SafetyVault.Api.Entities
{
    public class ChecklistItem
    {
        public int Id { get; set; }

        public int ChecklistId { get; set; }
        public Checklist Checklist { get; set; } = null!;

        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;

        public int TargetQuantity { get; set; }
        public int ActualQuantity { get; set; }
        public DateTimeOffset? PackedAt { get; set; }
    }
}
