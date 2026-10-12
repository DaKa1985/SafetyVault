using SafetyVault.Shared.Models;
using SafetyVault.Shared.Rules;
using SafetyVault.Wpf.Common;
using SafetyVault.Wpf.Exceptions;
using SafetyVault.Wpf.Models;
using SafetyVault.Wpf.Services;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;

namespace SafetyVault.Wpf.ViewModels
{
    public class StockViewModel : ValidatableBase
    {
        private readonly IStockBatchApiClient _stockBatchApiClient;
        private readonly IItemCatalogStore _itemCatalogStore;
        private readonly IDialogService _dialogService;
        private readonly TimeProvider _timeProvider;
        private bool _isBusy;
        private ItemDto? _selectedCatalogItem;
        private string _newQuantityText = "1";
        private DateTime? _newBestBefore;

        public StockViewModel(
            IStockBatchApiClient stockBatchApiClient,
            IItemCatalogStore itemCatalogStore,
            IDialogService dialogService,
            TimeProvider timeProvider)
        {
            _stockBatchApiClient = stockBatchApiClient ?? throw new ArgumentNullException(nameof(stockBatchApiClient));
            _itemCatalogStore = itemCatalogStore ?? throw new ArgumentNullException(nameof(itemCatalogStore));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            WithdrawOneCommand = new AsyncRelayCommand<StockBatch>(WithdrawOneAsync, CanWithdrawOne);
            AddOneCommand = new AsyncRelayCommand<StockBatch>(AddOneAsync, CanAddOne);
            BookInCommand = new AsyncRelayCommand(BookInAsync, CanBookIn);
        }

        public ObservableCollection<StockBatch> StockBatches { get; } = new ObservableCollection<StockBatch>();

        public ReadOnlyObservableCollection<ItemDto> AvailableItems => _itemCatalogStore.Items;

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

        public string NewQuantityText
        {
            get => _newQuantityText;
            set
            {
                if (_newQuantityText == value) return;
                _newQuantityText = value;
                OnPropertyChanged();
                ValidateNewQuantityText();
            }
        }

        public DateTime? NewBestBefore
        {
            get => _newBestBefore;
            set
            {
                if (_newBestBefore == value) return;
                _newBestBefore = value;
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

        public AsyncRelayCommand<StockBatch> WithdrawOneCommand { get; }
        public AsyncRelayCommand<StockBatch> AddOneCommand { get; }
        public AsyncRelayCommand BookInCommand { get; }

        private static StockBatch ToStockBatch(StockBatchDto dto, DateOnly today)
        {
            return new StockBatch
            {
                Id = dto.Id,
                ItemId = dto.ItemId,
                ItemName = dto.ItemName,
                Quantity = dto.Quantity,
                BestBefore = dto.BestBefore,
                DaysUntilExpiry = ExpiryRules.GetDaysUntilExpiry(dto.BestBefore, today),
                ExpiryStatus = ExpiryRules.GetExpiryStatus(dto.BestBefore, today)
            };
        }

        public async Task LoadAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                await RefreshAsync();
            }
            catch (HttpRequestException ex)
            {
                _dialogService.ShowError($"Die Anfrage an den Server ist fehlgeschlagen. - {ex.Message}");
            }
            catch (JsonException ex)
            {
                _dialogService.ShowError($"Die Antwort des Servers war nicht lesbar. - {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RefreshAsync()
        {
            var today = DateOnly.FromDateTime(_timeProvider.GetLocalNow().DateTime);
            var stockBatchDtos = await _stockBatchApiClient.GetStockBatchesAsync();
            StockBatches.Clear();
            foreach (var dto in stockBatchDtos)
            {
                StockBatches.Add(ToStockBatch(dto, today));
            }
        }

        private bool CanWithdrawOne(StockBatch? stockBatch)
        {
            return !IsBusy && stockBatch is not null && stockBatch.Quantity > 0;
        }

        private async Task WithdrawOneAsync(StockBatch? stockBatch)
        {
            if (stockBatch is null) return;

            var reloadNeeded = false;

            IsBusy = true;
            try
            {
                await _stockBatchApiClient.WithdrawAsync(stockBatch.Id, new WithdrawStockBatchDto { Quantity = 1 });
                await RefreshAsync();
            }
            catch (InsufficientQuantityException ex)
            {
                _dialogService.ShowError(ex.Message);
                reloadNeeded = true;
            }
            catch (HttpRequestException ex)
            {
                _dialogService.ShowError($"Die Anfrage an den Server ist fehlgeschlagen. - {ex.Message}");
            }
            catch (JsonException ex)
            {
                _dialogService.ShowError($"Die Antwort des Servers war nicht lesbar. - {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }

            if (reloadNeeded)
            {
                await LoadAsync();
            }
        }

        private bool CanAddOne(StockBatch? stockBatch)
        {
            return !IsBusy && stockBatch is not null;
        }

        private async Task AddOneAsync(StockBatch? stockBatch)
        {
            if (stockBatch is null) return;

            IsBusy = true;
            try
            {
                await _stockBatchApiClient.BookInAsync(new BookInStockBatchDto
                {
                    ItemId = stockBatch.ItemId,
                    Quantity = 1,
                    BestBefore = stockBatch.BestBefore
                });
                await RefreshAsync();
            }
            catch (HttpRequestException ex)
            {
                _dialogService.ShowError($"Die Anfrage an den Server ist fehlgeschlagen. - {ex.Message}");
            }
            catch (JsonException ex)
            {
                _dialogService.ShowError($"Die Antwort des Servers war nicht lesbar. - {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ValidateNewQuantityText()
        {
            var errors = new List<string>();
            if (!int.TryParse(NewQuantityText, out var quantity) || quantity <= 0)
            {
                errors.Add("Die Menge muss eine positive Zahl sein.");
            }
            else if (quantity > 1000)
            {
                errors.Add("Die Menge darf nicht größer als 1000 sein.");
            }

            SetErrors(nameof(NewQuantityText), errors);
        }

        private bool CanBookIn()
        {
            return !IsBusy && SelectedCatalogItem is not null && !HasErrors;
        }

        private async Task BookInAsync()
        {
            if (SelectedCatalogItem is null) return;

            IsBusy = true;
            try
            {
                await _stockBatchApiClient.BookInAsync(new BookInStockBatchDto
                {
                    ItemId = SelectedCatalogItem.Id,
                    Quantity = int.Parse(NewQuantityText),
                    BestBefore = NewBestBefore is null ? null : DateOnly.FromDateTime(NewBestBefore.Value)
                });
                NewQuantityText = "1";
                NewBestBefore = null;
                SelectedCatalogItem = null;
                await RefreshAsync();
            }
            catch (HttpRequestException ex)
            {
                _dialogService.ShowError($"Die Anfrage an den Server ist fehlgeschlagen. - {ex.Message}");
            }
            catch (JsonException ex)
            {
                _dialogService.ShowError($"Die Antwort des Servers war nicht lesbar. - {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
