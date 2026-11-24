

using SafetyVault.Shared.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace SafetyVault.Wpf.Services
{
    public class AuthApiClient : IAuthApiClient
    {
        private readonly HttpClient _httpClient;
        public AuthApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(HttpClient));
        }
        public async Task<bool> LoginAsync(string login, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", new LoginDto { Login = login, Password = password });

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return false;
            }

            response.EnsureSuccessStatusCode();

            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>()
                ?? throw new InvalidOperationException("Login-Antwort der Api war leer");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResponse.Token);
            return true;

        }
        public void Logout()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
