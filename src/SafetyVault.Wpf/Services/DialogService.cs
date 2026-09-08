using System.Windows;

namespace SafetyVault.Wpf.Services
{
    public class DialogService : IDialogService
    {
        public bool Confirm(string message)
        {
            return MessageBox.Show(message, "Bestätigung", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        public void ShowError(string message)
        {
            MessageBox.Show(message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
