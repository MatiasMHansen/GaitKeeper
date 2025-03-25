using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Shared.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace GaitKeeper.WebAssembly.Services
{
    public class SaveGaitDataService
    {
        private readonly HttpClient _httpClient;

        public SaveGaitDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> SaveGaitData(GaitDataDTO request)
        {
            var response = await _httpClient.PostAsJsonAsync("gaitsession/post", request);

            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response: {response.StatusCode}, Content: {responseContent}");

            return response.IsSuccessStatusCode;
        }
    }
}
