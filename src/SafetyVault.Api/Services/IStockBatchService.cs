using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Services
{
    public interface IStockBatchService
    {
        Task<IEnumerable<StockBatchDto>> GetAllAsync(int userId);
        Task<StockBatchDto?> GetByIdAsync(int userId, int id);
        Task<BookInStockBatchResult> BookInAsync(int userId, BookInStockBatchDto dto);
        Task<WithdrawStockBatchResult> WithdrawAsync(int userId, int stockBatchId, WithdrawStockBatchDto dto);
    }
}
