using SafetyVault.Shared.Models;
using SafetyVault.Wpf.Exceptions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace SafetyVault.Wpf.Services
{
    public class StockBatchApiClient : IStockBatchApiClient
    {
        private readonly HttpClient _httpClient;
        private const string StockBatchesPath = "api/stockbatches";
        public StockBatchApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient
                ?? throw new ArgumentNullException(nameof(httpClient));
            if (_httpClient.BaseAddress is null)
            {
                throw new InvalidOperationException("HttpClient BaseAddress is not set.");
            }
        }

        public async Task<IEnumerable<StockBatchDto>> GetStockBatchesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<StockBatchDto>>(StockBatchesPath)
                ?? Enumerable.Empty<StockBatchDto>();
        }
        public async Task<StockBatchDto> BookInAsync(BookInStockBatchDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync(StockBatchesPath, dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<StockBatchDto>()
                ?? throw new InvalidOperationException("The server returned an empty response.");
        }
        public async Task<StockBatchDto> WithdrawAsync(int stockBatchId, WithdrawStockBatchDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{StockBatchesPath}/{stockBatchId}/withdraw", dto);
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                throw new InsufficientQuantityException(dto.Quantity);
            }
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<StockBatchDto>()
                ?? throw new InvalidOperationException("The server returned an empty response.");
        }

    }
}
