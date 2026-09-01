using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace SafetyVault.Wpf.Services
{
    public class DialogService : IDialogService
    {
        public bool Confirm(string message)
        {
            return MessageBox.Show(message, "Bestätigung", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }
    }
}
