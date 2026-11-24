namespace SafetyVault.Wpf.Services
{
    public interface IAuthApiClient
    {
        Task<bool> LoginAsync(string login, string password);
        void Logout();
    }
}
