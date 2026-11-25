using System.ComponentModel.DataAnnotations;

namespace SafetyVault.Shared.Models
{
    public class CreateChecklistDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Length of 'Name' must not exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Length of 'Description' must not exceed 500 characters.")]
        public string? Description { get; set; }
    }
}
