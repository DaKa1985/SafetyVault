namespace SafetyVault.Api.Entities
{
    public class Checklist
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<ChecklistItem> ChecklistItems { get; set; } = new List<ChecklistItem>();
    }
}
