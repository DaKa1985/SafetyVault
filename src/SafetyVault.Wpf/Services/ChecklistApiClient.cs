using SafetyVault.Shared.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace SafetyVault.Wpf.Services
{
    public class ChecklistApiClient : IChecklistApiClient
    {
        private const string ChecklistsPath = "api/checklists";
        private readonly HttpClient _httpClient;
        public ChecklistApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient 
                ?? throw new ArgumentNullException(nameof(httpClient));
            if (_httpClient.BaseAddress is null)
            {
                throw new InvalidOperationException("HttpClient BaseAddress is not set.");
            }
        }

        public async Task<IEnumerable<ChecklistSummaryDto>> GetChecklistsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ChecklistSummaryDto>>(ChecklistsPath) 
                ?? Enumerable.Empty<ChecklistSummaryDto>();
        }
        public async Task<ChecklistDto?> GetChecklistByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<ChecklistDto>($"{ChecklistsPath}/{id}");
        }

        public async Task<ChecklistItemDto> AddItemToChecklistAsync(int checklistId, CreateChecklistItemDto dto)
        {
            var response =  await _httpClient.PostAsJsonAsync($"{ChecklistsPath}/{checklistId}/items", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ChecklistItemDto>()
                ?? throw new InvalidOperationException("The server returned an empty response.");
        }

        public async Task DeleteItemFromChecklistAsync(int checklistId, int checklistItemId)
        {
            var response = await _httpClient.DeleteAsync($"{ChecklistsPath}/{checklistId}/items/{checklistItemId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<ChecklistItemDto> UpdateChecklistItemAsync(int checklistId, int checklistItemId, UpdateChecklistItemDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"{ChecklistsPath}/{checklistId}/items/{checklistItemId}", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ChecklistItemDto>()
                ?? throw new InvalidOperationException("The server returned an empty response.");
        }
    }
}
