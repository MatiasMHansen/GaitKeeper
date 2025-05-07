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

        public string GetMarkerAxisDownloadUrl(Guid id)
        {
            //return $"https://localhost:7019/dataset/print/continuous/{id}"; // Url på Gateway

            throw new NotImplementedException("GetMarkerAxisDownloadUrl is not implemented yet.");
        }
    }
}
