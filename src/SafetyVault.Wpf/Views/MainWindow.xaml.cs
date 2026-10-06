using Microsoft.Extensions.Configuration;
using SafetyVault.Wpf.Services;
using SafetyVault.Wpf.ViewModels;
using System.Net.Http;
using System.Windows;

namespace SafetyVault.Wpf.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly ChecklistViewModel _checklistViewModel;
    private readonly ItemCatalogViewModel _itemCatalogViewModel;
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
        var dialogService = new DialogService();
        var apiClient = new ChecklistApiClient(httpClient);
        var itemApiClient = new ItemApiClient(httpClient);
        var itemCatalogStore = new ItemCatalogStore(itemApiClient);
        _checklistViewModel = new ChecklistViewModel(dialogService, apiClient, itemCatalogStore);
        _itemCatalogViewModel = new ItemCatalogViewModel(itemApiClient, dialogService, itemCatalogStore);
        DataContext = new MainViewModel(_checklistViewModel, _itemCatalogViewModel);
        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        await _checklistViewModel.LoadAsync();
        await _itemCatalogViewModel.LoadItemsAsync();
    }
}