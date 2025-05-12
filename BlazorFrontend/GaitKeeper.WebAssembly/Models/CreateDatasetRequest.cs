namespace GaitKeeper.WebAssembly.Models
{
    public class CreateDatasetRequest // kopi af CreateDatasetRequest fra DatasetService.API.DTOs.Requests
    {
        public string Name { get; set; }
        public List<Guid> PointDataIds { get; set; } = new();
    }
}
