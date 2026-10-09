using System.ComponentModel.DataAnnotations;

namespace SafetyVault.Shared.Models
{
    public class WithdrawStockBatchDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }
    }
}
