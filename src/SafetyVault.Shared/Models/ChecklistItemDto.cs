namespace SafetyVault.Shared.Models
{
    public class ChecklistItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int TargetQuantity { get; set; }
        public int ActualQuantity { get; set; }
        public DateTime? PackedAt { get; set; }
        public bool IsPacked => ActualQuantity >= TargetQuantity; 
        public int ChecklistId { get; set; }
    }
}