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
    private readonly ChecklistViewModel _viewModel;
    public MainWindow()
    {
        InitializeComponent();
        var httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7247/") };
        var apiClient = new ChecklistApiClient(httpClient);
        _viewModel = new ChecklistViewModel(new DialogService(), apiClient);
        DataContext = _viewModel;
        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        await _viewModel.LoadAsync();
    }
}