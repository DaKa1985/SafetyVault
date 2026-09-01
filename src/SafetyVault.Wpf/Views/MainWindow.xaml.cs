using SafetyVault.Wpf.Services;
using System.Windows;
using SafetyVault.Wpf.ViewModels;

namespace SafetyVault.Wpf.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new ChecklistViewModel(new DialogService());
    }
}