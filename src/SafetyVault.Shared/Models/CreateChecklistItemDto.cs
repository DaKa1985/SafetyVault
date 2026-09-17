 using System.ComponentModel.DataAnnotations;

namespace SafetyVault.Shared.Models
{
    public class CreateChecklistItemDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "ItemId must be greater than 0")]
        public int ItemId { get; set; }

        [Range(1, 1000, ErrorMessage = "TargetQuantity must be between 1 and 1000")]
        public int TargetQuantity { get; set; }
    }
}
