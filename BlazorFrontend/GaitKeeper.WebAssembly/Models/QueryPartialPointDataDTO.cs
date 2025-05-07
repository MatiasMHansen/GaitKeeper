namespace GaitKeeper.WebAssembly.Models
{
    public class QueryPartialPointDataDTO // Kopi af 'QueryPartialPointDataDTO' fra 'GaitPointData.Application.Query.QueryDTOs' & '.ValueObjectDTOs'
    {  
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string SubjectId { get; set; }
        public double PointFreq { get; set; }
        public double Duration { get; set; }
        public int Number { get; set; }
        public List<MarkerDTO> Markers { get; set; } = new();
    }

    public class MarkerDTO
    {
        public string Label { get; set; }
        public string UnitType { get; set; }
        public List<UnitDTO> Units { get; set; }
    }

    public class UnitDTO
    {
        public float? X { get; set; }
        public float? Y { get; set; }
        public float? Z { get; set; }
    }
}
