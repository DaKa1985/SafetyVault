using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Services
{
    public record AddChecklistItemResult(AddChecklistItemStatus Status, ChecklistItemDto? Item);
}
