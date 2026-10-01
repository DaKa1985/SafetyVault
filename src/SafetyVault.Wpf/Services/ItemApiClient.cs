using SafetyVault.Shared.Models;
using SafetyVault.Wpf.Exceptions;
using System.Net;
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

        public async Task<ItemDto> CreateItemAsync(CreateItemDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync(ItemsPath, dto);
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                throw new ItemAlreadyExistsException(dto.Name);
            }
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ItemDto>()
                ?? throw new InvalidOperationException("The server returned an empty response.");
        }
    }
}
