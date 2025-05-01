namespace DatasetService.API.DTOs.Requests
{
    public class GetPartialPointDataRequest
    {   // Skal synkroniseres med 'GetPartialPointDataRequest' i 'GaitPointData.API.DTOs'

        public List<Guid> PointDataIds { get; set; } = new();
        public List<string> Labels { get; set; } = new();
    }
}

// Bruges af 'PrintDatasetController' til at hente data fra 'GaitPointDataService'.