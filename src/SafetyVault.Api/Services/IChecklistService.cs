using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Services
{
    public interface IChecklistService
    {
        Task<IEnumerable<ChecklistSummaryDto>> GetAllAsync(int userId);
        Task<ChecklistDto?> GetByIdAsync(int userId, int id);
        Task<ChecklistItemDto?> GetItemAsync(int checklistId, int checklistItemId);
        Task<AddChecklistItemResult> AddItemAsync(int checklistId, CreateChecklistItemDto dto);
        Task<ChecklistItemDto?> UpdateItemAsync(int checklistId, int checklistItemId, UpdateChecklistItemDto dto);
        Task<bool> DeleteItemAsync(int checklistId, int checklistItemId);
    }
}
