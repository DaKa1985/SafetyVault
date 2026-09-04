using System.ComponentModel.DataAnnotations;

namespace SafetyVault.Shared.Models
{
    public class CreateChecklistItemDto
    {
        [Required(ErrorMessage="Name wird benötigt")]
        [MaxLength(100, ErrorMessage = "Name darf maximal 100 Zeichen lang sein")]
        public string Name { get; set; } = string.Empty;
        [MaxLength(500, ErrorMessage = "Beschreibung darf maximal 500 Zeichen lang sein")]
        public string? Description { get; set; }
        [Range(1, 1000, ErrorMessage = "Menge muss zwischen 1 und 1000 liegen")]
        public int Quantity { get; set; } = 1;

    }
}
