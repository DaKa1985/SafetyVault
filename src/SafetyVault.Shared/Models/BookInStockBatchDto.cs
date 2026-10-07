using System.ComponentModel.DataAnnotations;

namespace SafetyVault.Shared.Models
{
    public class BookInStockBatchDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "ItemId must be greater than 0")]
        public int ItemId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }
        public DateOnly? BestBefore { get; set; }
    }
}
