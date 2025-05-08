using Shared.DTOs;
using System.Net.Http.Json;

namespace GaitKeeper.WebAssembly.Services
{
    public class GaitDataOrchestratorService
    {
        private readonly HttpClient _httpClient;

        public GaitDataOrchestratorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SaveGaitDataAsync(CreateGaitSessionDTO createDto)
        {
            var response = await _httpClient.PostAsJsonAsync("gaitdata/post", createDto);
            // Brug response til at håndtere succes eller fejl
        }
    }
}
