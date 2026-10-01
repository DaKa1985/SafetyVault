using SafetyVault.Shared.Models;
using SafetyVault.Wpf.Common;
using SafetyVault.Wpf.Exceptions;
using SafetyVault.Wpf.Services;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;

namespace SafetyVault.Wpf.ViewModels
{
    public class ItemCatalogViewModel : ValidatableBase
    {
        private string _newItemName = string.Empty;
        private readonly IItemApiClient _itemApiClient;
        private readonly IDialogService _dialogService;
        private bool _isBusy;

        public ItemCatalogViewModel(IItemApiClient itemApiClient, IDialogService dialogService)
        {
            _itemApiClient = itemApiClient ?? throw new ArgumentNullException(nameof(itemApiClient));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            CreateItemCommand = new AsyncRelayCommand(CreateItemAsync, CanCreateItem);
        }

        public AsyncRelayCommand CreateItemCommand { get; }
        public ObservableCollection<ItemDto> Items { get; } = new();
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

        private void ValidateNewItemName()
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(NewItemName))
            {
                errors.Add("Der Artikelname darf nicht leer sein.");
            }
            else if (NewItemName.Trim().Length > 100)
            {
                errors.Add("Der Artikelname darf nicht länger als 100 Zeichen sein.");
            }
            SetErrors(nameof(NewItemName), errors);
        }

        private bool CanCreateItem() =>
            !IsBusy && !HasErrors && !string.IsNullOrWhiteSpace(NewItemName);

        private async Task CreateItemAsync()
        {
            if (!CanCreateItem())
            {
                return;
            }
            IsBusy = true;
            try
            {
                var newItemDto = new CreateItemDto { Name = NewItemName.Trim() };
                await _itemApiClient.CreateItemAsync(newItemDto);
                await LoadItemsAsync();
                NewItemName = string.Empty;
                ClearErrors(nameof(NewItemName));
            }
            catch (ItemAlreadyExistsException ex)
            {
                _dialogService.ShowError($"Artikel konnte nicht erstellt werden: {ex.Message}");
            }
            catch (HttpRequestException ex)
            {
                _dialogService.ShowError($"Netzwerkfehler beim Erstellen des Artikels: {ex.Message}");
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Unbekannter Fehler beim Erstellen des Artikels: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                CommandManager.InvalidateRequerySuggested();
            }
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