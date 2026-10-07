using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Services
{
    public interface IStockBatchService
    {
        Task<IEnumerable<StockBatchDto>> GetAllAsync();
        Task<StockBatchDto?> GetByIdAsync(int id);
    }
}
