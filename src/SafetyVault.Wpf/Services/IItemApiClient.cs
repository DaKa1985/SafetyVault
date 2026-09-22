using SafetyVault.Shared.Models;

namespace SafetyVault.Wpf.Services
{
    public interface IItemApiClient
    {
        Task<IEnumerable<ItemDto>> GetItemsAsync();
    }
}
