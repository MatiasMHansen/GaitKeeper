namespace GaitKeeper.WebAssembly.Models
{
    public class PrintMarkerAxisRequest // kopi af 'PrintMarkerAxisRequest' fra 'DatasetService.API.DTOs.Requests'
    {
        public Guid Id { get; set; }
        public string MarkerLabel { get; set; }
        public char Axis { get; set; } // Skal være uppercase X, Y eller Z
    }
}
