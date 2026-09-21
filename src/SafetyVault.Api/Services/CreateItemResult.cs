using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Services
{
    public record CreateItemResult(CreateItemStatus Status, ItemDto? Item);
}
