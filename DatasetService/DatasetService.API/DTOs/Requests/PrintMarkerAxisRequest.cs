namespace DatasetService.API.DTOs.Requests
{
    public class PrintMarkerAxisRequest
    {
        public Guid Id { get; set; }
        public string MarkerLabel { get; set; }
        public char Axis { get; set; } // Skal være uppercase X, Y eller Z
    }
}
