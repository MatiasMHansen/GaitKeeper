using GaitKeeper.WebAssembly.Models;
using Shared.DTOs.RawGaitData;
using System.Net.Http.Json;

namespace GaitKeeper.WebAssembly.Services
{
    public class GaitSessionService
    {
        private readonly HttpClient _httpClient;
        public GaitSessionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<RawGaitSessionDTO> GetRawGaitSessionAsync(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name is required.", nameof(fileName));

            var response = await _httpClient.GetFromJsonAsync<RawGaitSessionDTO>($"gaitsession/raw/{fileName}");
            return response ?? new RawGaitSessionDTO();
        }

        public async Task<List<QueryGaitSessionDTO>> GetAllGaitSessionsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<QueryGaitSessionDTO>>("gaitsession/all");
            return response ?? new List<QueryGaitSessionDTO>();
        }
    }
}
