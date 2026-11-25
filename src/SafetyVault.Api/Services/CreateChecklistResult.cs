using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Services
{
    public record CreateChecklistResult(CreateChecklistStatus Status, ChecklistDto? Checklist);
}
