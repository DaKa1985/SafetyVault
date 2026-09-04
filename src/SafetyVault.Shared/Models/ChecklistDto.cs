namespace SafetyVault.Shared.Models
{
    public class ChecklistDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<ChecklistItemDto> Items { get; set; } = new List<ChecklistItemDto>();
    }
}
