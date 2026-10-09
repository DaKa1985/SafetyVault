using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Services
{
    public record WithdrawStockBatchResult(WithdrawStockBatchStatus Status, StockBatchDto? StockBatch);
}
