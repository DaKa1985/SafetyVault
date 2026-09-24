using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Services
{
    public interface IChecklistService
    {
        Task<IEnumerable<ChecklistSummaryDto>> GetAllAsync();
        Task<ChecklistDto?> GetByIdAsync(int id);
        Task<ChecklistItemDto?> GetItemAsync(int checklistId, int checklistItemId);
        Task<AddChecklistItemResult> AddItemAsync(int checklistId, CreateChecklistItemDto dto);
        Task<ChecklistItemDto?> UpdateItemAsync(int checklistId, int checklistItemId, UpdateChecklistItemDto dto);
        Task<bool> DeleteItemAsync(int checklistId, int checklistItemId);
    }
}
