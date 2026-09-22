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
        private string _newItemName = string.Empty;
        private string _newItemQuantityText = "1";
        private int _newItemQuantity;
        private readonly IDialogService _dialogService;
        private readonly IChecklistApiClient _apiClient;
        private readonly IItemApiClient _itemApiClient;

        public ChecklistViewModel(IDialogService dialogService, IChecklistApiClient apiClient, IItemApiClient itemApiClient)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _itemApiClient = itemApiClient ?? throw new ArgumentNullException(nameof(itemApiClient));
            AddItemCommand = new RelayCommand(AddItem, CanAddItem);
            DeleteItemCommand = new RelayCommand<ChecklistItem>(DeleteItem, CanDeleteItem);
            ValidateNewItemName();
            ValidateNewItemQuantityText();

        }
        public string NewItemName
        {
            get => _newItemName;
            set
            {
                if (_newItemName == value) return;
                _newItemName = value;
                OnPropertyChanged();
                ValidateNewItemName();
            }
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
        public ICommand AddItemCommand { get; }
        public ICommand DeleteItemCommand { get; }

        private void AddItem()
        {
            Items.Add(new ChecklistItem { Name = NewItemName, TargetQuantity = _newItemQuantity, ActualQuantity = 0 });
            NewItemName = string.Empty;
            NewItemQuantityText = "1";

        }
        private bool CanAddItem() =>
            !HasErrors;
        private void DeleteItem(ChecklistItem? item)
        {
            //vorher eine Bestätigung einholen, ob der Benutzer das Item wirklich löschen möchte
            if (item is null) return;
            if (!_dialogService.Confirm($"Möchten Sie die Position '{item.Name}' wirklich löschen?"))
            {
                return;
            }
            Items.Remove(item);
        }
        private bool CanDeleteItem(ChecklistItem? item) =>
            item is not null;
        private void ValidateNewItemName()
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(NewItemName))
            {
                errors.Add("Der Name darf nicht leer sein.");
            }
            if (NewItemName.Length > 100)
            {
                errors.Add("Der Name darf nicht länger als 100 Zeichen sein.");
            }
            SetErrors(nameof(NewItemName), errors);
        }
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
    }
}
