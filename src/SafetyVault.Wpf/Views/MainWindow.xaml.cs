using Microsoft.Extensions.Configuration;
using SafetyVault.Wpf.Services;
using SafetyVault.Wpf.ViewModels;
using System.Net.Http;
using System.Windows;

namespace SafetyVault.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
            var baseAddress = configuration["Api:BaseAddress"]
                ?? throw new InvalidOperationException("API base address is not configured.");
            var httpClient = new HttpClient { BaseAddress = new Uri(baseAddress) };

            var authApiClient = new AuthApiClient(httpClient);
            var loginViewModel = new LoginViewModel(authApiClient);

            var dialogService = new DialogService();
            var apiClient = new ChecklistApiClient(httpClient);
            var itemApiClient = new ItemApiClient(httpClient);
            var itemCatalogStore = new ItemCatalogStore(itemApiClient);
            var stockBatchApiClient = new StockBatchApiClient(httpClient);

            var checklistViewModel = new ChecklistViewModel(dialogService, apiClient, itemCatalogStore);
            var itemCatalogViewModel = new ItemCatalogViewModel(itemApiClient, dialogService, itemCatalogStore);
            var stockViewModel = new StockViewModel(stockBatchApiClient, itemCatalogStore, dialogService, TimeProvider.System);

            DataContext = new MainViewModel(checklistViewModel, itemCatalogViewModel, stockViewModel, loginViewModel);
        }
    }
}