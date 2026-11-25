using SafetyVault.Wpf.Common;
using SafetyVault.Wpf.Services;
using System.ComponentModel;
using System.Net.Http;
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
            LoginCommand = new AsyncRelayCommand(LoginAsync, () => !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password));
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
        public AsyncRelayCommand LoginCommand { get; }

        public event EventHandler? LoggedIn;

        public event PropertyChangedEventHandler? PropertyChanged;

        private async Task LoginAsync()
        {
            ErrorMessage = null;
            try
            {
                bool loginOk = await _authApiClient.LoginAsync(Login, Password);
                if (loginOk)
                {
                    Password = string.Empty;
                    LoggedIn?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    ErrorMessage = "Benutzername oder Kennwort falsch.";
                }
            }
            catch (HttpRequestException )
            {
                ErrorMessage = "Api nicht erreichbar. Bitte später erneut versuchen.";
            }
        }

        public void Logout()
        {
            _authApiClient.Logout();
            Login = string.Empty;
            Password = string.Empty;
            ErrorMessage = null;
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    } 
}
