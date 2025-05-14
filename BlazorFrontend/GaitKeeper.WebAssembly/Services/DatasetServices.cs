using GaitKeeper.WebAssembly.Models;
using System.Net.Http.Json;

namespace GaitKeeper.WebAssembly.Services
{
    public class DatasetServices
    {
        private readonly HttpClient _httpClient;

        public DatasetServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<QueryPreviewDatasetDTO>> GetAllDatasetsAsync()
        {
            var datasets = await _httpClient.GetFromJsonAsync<List<QueryPreviewDatasetDTO>>("dataset/read/all");
            return datasets ?? new List<QueryPreviewDatasetDTO>();
        }

        public async Task<QueryDatasetDTO> GetOneDatasetAsync(Guid id)
        {
            var dataset = await _httpClient.GetFromJsonAsync<QueryDatasetDTO>($"dataset/read/{id}");
            return dataset ?? new QueryDatasetDTO();
        }

        public string GetCharacteristicsDownloadUrl(Guid id)
        {
            return $"https://localhost:7019/dataset/print/characteristics/{id}"; // Url på Gateway
        }

        public async Task<(byte[] Content, string FileName)> GetMarkerAxisFileAsync(Guid datasetId, string markerLabel, char axis)
        {
            var request = new PrintMarkerAxisRequest
            {
                Id = datasetId,
                MarkerLabel = markerLabel,
                Axis = axis
            };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7019/dataset/print/marker-axis", request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Request failed: {response.StatusCode}");

            var content = await response.Content.ReadAsByteArrayAsync();
            var fileName = response.Content.Headers.ContentDisposition?.FileName?.Trim('"') ?? "download.csv";

            return (content, fileName);
        }

        public async Task SaveDatasetAsync(CreateDatasetRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("dataset/save", request);
            // Brug response til at håndtere succes eller fejl
        }
    }
}
