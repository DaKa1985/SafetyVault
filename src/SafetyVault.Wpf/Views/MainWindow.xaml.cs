using SafetyVault.Wpf.Services;
using System.Windows;
using SafetyVault.Wpf.ViewModels;
using System.Net.Http;

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
        var httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7247/") };
        var apiClient = new ChecklistApiClient(httpClient);
        var itemApiClient = new ItemApiClient(httpClient);
        _checklistViewModel = new ChecklistViewModel(new DialogService(), apiClient, itemApiClient);
        _itemCatalogViewModel = new ItemCatalogViewModel(itemApiClient, new DialogService());
        DataContext = new MainViewModel(_checklistViewModel, _itemCatalogViewModel);
        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        await _checklistViewModel.LoadAsync();
        await _itemCatalogViewModel.LoadItemsAsync();
    }
}