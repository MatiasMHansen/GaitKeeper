namespace Shared.DTOs.PubSubDTOs
{
    public class SaveStatusDTO
    {
        public string Service { get; set; }
        public Guid CorrelationId { get; set; }
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
