using SafetyVault.Shared.Models;
namespace SafetyVault.Wpf.Services
{
    public interface IChecklistApiClient
    {
        Task<IEnumerable<ChecklistSummaryDto>> GetChecklistsAsync();
        Task<ChecklistDto?> GetChecklistByIdAsync(int id);
        Task<ChecklistDto> CreateChecklistAsync(CreateChecklistDto dto);
        Task<ChecklistItemDto> AddItemToChecklistAsync(int checklistId, CreateChecklistItemDto dto);
        Task DeleteItemFromChecklistAsync(int checklistId, int checklistItemId);
        Task<ChecklistItemDto> UpdateChecklistItemAsync(int checklistId, int checklistItemId, UpdateChecklistItemDto dto);

    }
}
