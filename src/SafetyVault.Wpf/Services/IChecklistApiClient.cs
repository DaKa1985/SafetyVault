using SafetyVault.Shared.Models;
using SafetyVault.Wpf.Models;
namespace SafetyVault.Wpf.Services
{
    public interface IChecklistApiClient
    {
        Task<IEnumerable<ChecklistSummaryDto>> GetChecklistsAsync();
        Task<ChecklistDto?> GetChecklistByIdAsync(int id);
        Task<ChecklistItemDto> AddItemToChecklistAsync(int checklistId, CreateChecklistItemDto dto);
    }
}
