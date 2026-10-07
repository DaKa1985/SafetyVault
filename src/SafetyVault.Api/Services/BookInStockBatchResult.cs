using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Services
{
    public record BookInStockBatchResult(BookInStockBatchStatus Status, StockBatchDto? StockBatch);
}
