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
            if (_httpClient.BaseAddress == null)
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
    }
}
