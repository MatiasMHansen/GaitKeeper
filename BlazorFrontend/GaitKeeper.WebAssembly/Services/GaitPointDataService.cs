using GaitKeeper.WebAssembly.Models;
using System.Net.Http.Json;

namespace GaitKeeper.WebAssembly.Services
{
    public class GaitPointDataService
    {
        private readonly HttpClient _httpClient;
        public GaitPointDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<QueryPartialPointDataDTO>> GetPartialPointDataAsync(List<Guid> pointDataIds, string label)
        {
            var requestBody = new
            {
                PointDataIds = pointDataIds,
                Labels = new List<string> { label }
            };

            var response = await _httpClient.PostAsJsonAsync("partialpointdata/by-labels", requestBody);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<List<QueryPartialPointDataDTO>>();
                return data ?? new List<QueryPartialPointDataDTO>();
            }

            return new List<QueryPartialPointDataDTO>();
        }
    }
}
