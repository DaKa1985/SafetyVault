using SafetyVault.Shared.Models;

namespace SafetyVault.Wpf.Services
{
    public interface IStockBatchApiClient
    {
        Task<IEnumerable<StockBatchDto>> GetStockBatchesAsync();
        Task<StockBatchDto> BookInAsync(BookInStockBatchDto dto);
        Task<StockBatchDto> WithdrawAsync(int stockBatchId, WithdrawStockBatchDto dto);
    }
}
