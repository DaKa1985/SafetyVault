using SafetyVault.Wpf.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SafetyVault.Wpf.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IAuthApiClient _authApiClient;
        private string _login = string.Empty;
        private string _password = string.Empty;
        private string? _errorMessage;

        public LoginViewModel(IAuthApiClient authApiClient)
        {
            _authApiClient = authApiClient ?? throw new ArgumentNullException(nameof(authApiClient));
        }

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
        public string? ErrorMessage
        {
            get => _errorMessage;
            private set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    } 
}
