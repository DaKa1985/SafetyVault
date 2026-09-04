namespace SafetyVault.Shared.Models
{
    public class CreateChecklistItemDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Quantity { get; set; } = 1;

    }
}
