using SafetyVault.Shared.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace SafetyVault.Wpf.Services
{
    public class ItemApiClient : IItemApiClient
    {
        private readonly HttpClient _httpClient;
        private const string ItemsPath = "api/items";

        public ItemApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient
                ?? throw new ArgumentNullException(nameof(httpClient));
            if (_httpClient.BaseAddress is null)
            {
                throw new InvalidOperationException("HttpClient BaseAddress is not set.");
            }
        }

        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ItemDto>>(ItemsPath)
                ?? Enumerable.Empty<ItemDto>();
        }
    }
}
