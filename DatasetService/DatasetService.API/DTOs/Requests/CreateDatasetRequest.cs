namespace DatasetService.API.DTOs.Requests
{
    public class CreateDatasetRequest
    {
        public string Name { get; set; }
        public List<Guid> PointDataIds { get; set; } = new();
    }
}

// Requestet initieres af brugeren, som sidder i UI og vælger hvilke GaitSessions og Labels (markørere), der skal bruges til at generere en dataset.