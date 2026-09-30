using SafetyVault.Shared.Models;
using SafetyVault.Wpf.Common;
using SafetyVault.Wpf.Services;
using System.Collections.ObjectModel;

namespace SafetyVault.Wpf.ViewModels
{
    public class ItemCatalogViewModel : ValidatableBase
    {
        public ObservableCollection<ItemDto> Items { get; } = new();

        private readonly IItemApiClient _itemApiClient;
        private readonly IDialogService _dialogService;

        public ItemCatalogViewModel(IItemApiClient itemApiClient, IDialogService dialogService)
        {
            _itemApiClient = itemApiClient ?? throw new ArgumentNullException(nameof(itemApiClient));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        }

        public async Task LoadItemsAsync()
        {
            try
            {
                var items = await _itemApiClient.GetItemsAsync();
                Items.Clear();
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Probleme beim Laden der Artikel - {ex.Message}");
            }
        }
    }
}