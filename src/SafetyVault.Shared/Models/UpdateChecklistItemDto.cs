using System.ComponentModel.DataAnnotations;

namespace SafetyVault.Shared.Models
{
    public class UpdateChecklistItemDto
    {
        [Range(0, int.MaxValue, ErrorMessage = "ActualQuantity must be greater or equal to 0")]
        public int ActualQuantity { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "TargetQuantity must be greater than 0.")]
        public int TargetQuantity { get; set; }
    }
}