namespace GaitPointData.API.DTOs
{
    public class GetPartialPointDataRequest
    {   // Skal synkroniseres med 'GetPartialPointDataRequest' i 'DatasetService.API.DTOs.Requests'

        public List<Guid> PointDataIds { get; set; } = new();
        public List<string> Labels { get; set; } = new();
    }
}
