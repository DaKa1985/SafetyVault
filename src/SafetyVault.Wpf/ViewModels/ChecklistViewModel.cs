using SafetyVault.Shared.Models;
using SafetyVault.Wpf.Common;
using SafetyVault.Wpf.Models;
using SafetyVault.Wpf.Services;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Input;

namespace SafetyVault.Wpf.ViewModels
{
    public class ChecklistViewModel : ValidatableBase
    {
        private string _newItemQuantityText = "1";
        private int _newItemQuantity;
        private readonly IDialogService _dialogService;
        private readonly IChecklistApiClient _apiClient;
        private readonly IItemApiClient _itemApiClient;
        private ItemDto? _selectedCatalogItem;
        private bool _isBusy;

        public ChecklistViewModel(IDialogService dialogService, IChecklistApiClient apiClient, IItemApiClient itemApiClient)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _itemApiClient = itemApiClient ?? throw new ArgumentNullException(nameof(itemApiClient));
            AddItemCommand = new AsyncRelayCommand(AddItemAsync, CanAddItem);
            DeleteItemCommand = new AsyncRelayCommand<ChecklistItem>(DeleteItemAsync, CanDeleteItem);
            IncreaseQuantityCommand = new AsyncRelayCommand<ChecklistItem>(IncreaseQuantityAsync, CanIncreaseQuantity);
            DecreaseQuantityCommand = new AsyncRelayCommand<ChecklistItem>(DecreaseQuantityAsync, CanDecreaseQuantity);
            PackItemCommand = new AsyncRelayCommand<ChecklistItem>(PackItemAsync, CanPackItem);
            ValidateNewItemQuantityText();
        }

        public string NewItemQuantityText
        {
            get => _newItemQuantityText;
            set
            {
                if (_newItemQuantityText == value) return;
                _newItemQuantityText = value;
                OnPropertyChanged();
                ValidateNewItemQuantityText();
            }
        }
        public ObservableCollection<ChecklistItem> Items { get; } = new();
        public ObservableCollection<ItemDto> AvailableItems { get; } = new();
        public ItemDto? SelectedCatalogItem
        {
            get => _selectedCatalogItem;
            set
            {
                if (_selectedCatalogItem == value) return;
                _selectedCatalogItem = value;
                OnPropertyChanged();
            }
        }
        public bool IsBusy
        {
            get => _isBusy;
            private set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                OnPropertyChanged();
            }
        }
        public ICommand AddItemCommand { get; }
        public ICommand DeleteItemCommand { get; }
        public ICommand IncreaseQuantityCommand { get; } 
        public ICommand DecreaseQuantityCommand { get; } 
        public ICommand PackItemCommand { get; }
        private async Task AddItemAsync()
        {
            if (SelectedCatalogItem is null) return;
            IsBusy = true;
            try
            {
                var dto = new CreateChecklistItemDto { ItemId = SelectedCatalogItem.Id, TargetQuantity = _newItemQuantity };
                var newItem = await _apiClient.AddItemToChecklistAsync(1, dto);
                NewItemQuantityText = "1";
                Items.Add(new ChecklistItem
                {
                    Name = newItem.Name,
                    Description = newItem.Description,
                    TargetQuantity = newItem.TargetQuantity,
                    ActualQuantity = newItem.ActualQuantity,
                    PackedAt = newItem.PackedAt?.LocalDateTime,
                    Id = newItem.Id,
                    ChecklistId = newItem.ChecklistId,
                    ItemId = newItem.ItemId
                });
            }
            catch (HttpRequestException ex)
            {
                _dialogService.ShowError($"Der Artikel konnte nicht hinzugefügt werden. - {ex.Message}");
            }
            catch (JsonException ex)
            {
                _dialogService.ShowError($"Die Antwort des Servers war nicht lesbar.- {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                _dialogService.ShowError($"Fehler beim Hinzufügen des Artikels: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
        private bool CanAddItem() =>
            !IsBusy && SelectedCatalogItem is not null && !HasErrors;
        private async Task DeleteItemAsync (ChecklistItem? item)
        {
            //vorher eine Bestätigung einholen, ob der Benutzer das Item wirklich löschen möchte
            if (item is null) return;
            if (!_dialogService.Confirm($"Möchten Sie die Position '{item.Name}' wirklich löschen?"))
            {
                return;
            }
            IsBusy = true;
            try
            {
                await _apiClient.DeleteItemFromChecklistAsync(item.ChecklistId, item.Id);
                Items.Remove(item);
            }
            catch (HttpRequestException ex)
            {
                _dialogService.ShowError($"Der Artikel konnte nicht gelöscht werden. - {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
        
        private bool CanDeleteItem(ChecklistItem? item) =>
            !IsBusy && item is not null;
        
        private void ValidateNewItemQuantityText()
        {
            var errors = new List<string>();
            if (!int.TryParse(NewItemQuantityText, out var quantity) || quantity <= 0)
            {
                errors.Add("Die Menge muss eine positive Zahl sein.");
            }
            else if (quantity > 1000)
            {
                errors.Add("Die Menge darf nicht größer als 1000 sein.");
            }
            else
            {
                _newItemQuantity = quantity;
            }
            SetErrors(nameof(NewItemQuantityText), errors);
        }
        public async Task LoadAsync()
        {
            try
            {
                var checklist = await _apiClient.GetChecklistByIdAsync(1); // Feste Id bis zur Checklisten-Auswahl in Woche 2
                if (checklist is null) return;
                Items.Clear();
                foreach (var item in checklist.Items)
                {
                    Items.Add(new ChecklistItem
                    {
                        Name = item.Name,
                        Description = item.Description,
                        TargetQuantity = item.TargetQuantity,
                        ActualQuantity = item.ActualQuantity,
                        PackedAt = item.PackedAt?.LocalDateTime,
                        Id = item.Id,
                        ChecklistId = item.ChecklistId,
                        ItemId = item.ItemId
                    });
                }
                var catalog = await _itemApiClient.GetItemsAsync();
                AvailableItems.Clear();
                foreach (var item in catalog)
                {
                    AvailableItems.Add(item);
                }
            }
            catch (HttpRequestException ex)
            {
                _dialogService.ShowError($"Die Anwendung konnte den Server nicht erreichen. Prüfen Sie, ob die API läuft. - {ex.Message}");
            }
            catch (JsonException ex)
            {
                _dialogService.ShowError($"Die Antwort des Servers war nicht lesbar.- {ex.Message}");
            }
        }
        private async Task UpdateActualQuantityAsync(ChecklistItem? item, int newQuantity)
        {
            if (item is null) return;
            IsBusy = true;
            try
            {
                var dto = new UpdateChecklistItemDto
                {
                    TargetQuantity = item.TargetQuantity,
                    ActualQuantity = newQuantity
                };
                var updatedItem = await _apiClient.UpdateChecklistItemAsync(item.ChecklistId, item.Id, dto);
                item.ActualQuantity = updatedItem.ActualQuantity;
                item.PackedAt = updatedItem.PackedAt?.LocalDateTime;
            }
            catch (HttpRequestException ex)
            {
                _dialogService.ShowError($"Der Artikel konnte nicht aktualisiert werden. - {ex.Message}");
            }
            catch (JsonException ex)
            {
                _dialogService.ShowError($"Die Antwort des Servers war nicht lesbar.- {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                _dialogService.ShowError($"Fehler beim Aktualisieren des Artikels: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanIncreaseQuantity(ChecklistItem? item) => 
            !IsBusy && item is not null && !item.IsPacked;
        private async Task IncreaseQuantityAsync(ChecklistItem? item)
        {
            if (item is null) return;
            await UpdateActualQuantityAsync(item, item.ActualQuantity + 1);
        }
        private bool CanDecreaseQuantity(ChecklistItem? item) =>
            !IsBusy && item is not null && item.ActualQuantity > 0;
        private async Task DecreaseQuantityAsync(ChecklistItem? item)
        {
            if (item is null) return;
            await UpdateActualQuantityAsync(item, item.ActualQuantity - 1);
        }
        private bool CanPackItem(ChecklistItem? item) =>
            !IsBusy && item is not null && !item.IsPacked;
        private async Task PackItemAsync(ChecklistItem? item)
        {
            if (item is null) return;
            await UpdateActualQuantityAsync(item, item.TargetQuantity);
        }
    }
}
