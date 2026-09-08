namespace SafetyVault.Wpf.Services
{
    public interface IDialogService
    {
        bool Confirm(string message);
        void ShowError(string message);
    }
}