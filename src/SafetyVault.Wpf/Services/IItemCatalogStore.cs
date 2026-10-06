using SafetyVault.Shared.Models;
using System.Collections.ObjectModel;

namespace SafetyVault.Wpf.Services
{
    public interface IItemCatalogStore
    {
        ReadOnlyObservableCollection<ItemDto> Items { get; }
        Task LoadAsync();
    }
}
