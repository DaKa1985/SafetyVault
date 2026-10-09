using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Services
{
    public interface IStockBatchService
    {
        Task<IEnumerable<StockBatchDto>> GetAllAsync();
        Task<StockBatchDto?> GetByIdAsync(int id);
        Task<BookInStockBatchResult> BookInAsync(BookInStockBatchDto dto);
        Task<WithdrawStockBatchResult> WithdrawAsync(int stockBatchId, WithdrawStockBatchDto dto);
    }
}
