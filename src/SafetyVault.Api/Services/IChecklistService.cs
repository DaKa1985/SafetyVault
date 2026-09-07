using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Services
{
    public interface IChecklistService
    {
        IEnumerable<ChecklistSummaryDto> GetAll();
        ChecklistDto? GetById(int id);
        ChecklistItemDto? GetItem(int checklistId, int itemId);
        ChecklistItemDto? AddItem(int checklistId, CreateChecklistItemDto dto);
        bool UpdateItem(int checklistId, int itemId, UpdateChecklistItemDto dto);
        bool DeleteItem(int checklistId, int itemId);
    }
}
