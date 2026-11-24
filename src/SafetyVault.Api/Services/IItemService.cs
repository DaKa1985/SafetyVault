using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Services
{
    public interface IItemService
    {
        Task<IEnumerable<ItemDto>> GetAllAsync(int userId);
        Task<ItemDto?> GetByIdAsync(int userId, int id);
        Task<CreateItemResult> CreateAsync(int userId, CreateItemDto dto);

    }
}
