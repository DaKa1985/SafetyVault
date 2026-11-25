using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SafetyVault.Wpf.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            private set
            {
                if (_isLoggedIn != value)
                {
                    _isLoggedIn = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsLoggedOut));
                }
            }
        }
        public bool IsLoggedOut => !IsLoggedIn;
        public ChecklistViewModel Checklists { get; }
        public ItemCatalogViewModel ItemCatalog { get; }
        public StockViewModel Stock { get; }
        public LoginViewModel Login { get; }
        public event PropertyChangedEventHandler? PropertyChanged;

        public MainViewModel(ChecklistViewModel checklists, ItemCatalogViewModel itemCatalog, StockViewModel stock, LoginViewModel login)
        {
            Checklists = checklists ?? throw new ArgumentNullException(nameof(checklists));
            ItemCatalog = itemCatalog ?? throw new ArgumentNullException(nameof(itemCatalog));
            Stock = stock ?? throw new ArgumentNullException(nameof(stock));
            Login = login ?? throw new ArgumentNullException(nameof(login));
            Login.LoggedIn += OnLoggedIn;
        }
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private async void OnLoggedIn(object? sender, EventArgs e)
        {
            IsLoggedIn = true;

            await Checklists.LoadAsync();
            await ItemCatalog.LoadItemsAsync();
            await Stock.LoadAsync();
        }
    }
}