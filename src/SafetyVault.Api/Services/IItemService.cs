using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Services
{
    public interface IItemService
    {
        Task<IEnumerable<ItemDto>> GetAllAsync();
        Task<ItemDto?> GetByIdAsync(int id);
        Task<CreateItemResult> CreateAsync(CreateItemDto dto);

    }
}
