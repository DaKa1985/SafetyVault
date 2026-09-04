namespace SafetyVault.Shared.Models
{
    public class ChecklistSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ItemCount { get; set; }

    }
}
