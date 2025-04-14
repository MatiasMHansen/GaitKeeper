namespace GaitPointData.API.DTO_s.Requests
{
    public class GetPartialPointDataRequest
    {
        public List<Guid> PointDataIds { get; set; } = new();
        public List<string> Labels { get; set; } = new();
    }
}
