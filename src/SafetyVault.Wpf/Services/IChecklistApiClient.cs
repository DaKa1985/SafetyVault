using SafetyVault.Shared.Models;
namespace SafetyVault.Wpf.Services
{
    public interface IChecklistApiClient
    {
        Task<IEnumerable<ChecklistSummaryDto>> GetChecklistsAsync();
        Task<ChecklistDto?> GetChecklistByIdAsync(int id);
    }
}
