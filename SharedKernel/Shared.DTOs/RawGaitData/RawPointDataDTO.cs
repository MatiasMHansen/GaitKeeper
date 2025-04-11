namespace Shared.DTOs.RawGaitData
{
    public class RawPointDataDTO // Aggregate
    {
        public int StartFrame { get; set; }
        public int EndFrame { get; set; }
        public double PointFreq { get; set; }
        public List<string> AllLabels { get; set; } = new List<string>();
        public List<MarkerDTO> Markers { get; set; } = new List<MarkerDTO>();
        public GaitEventsDTO GaitEvents { get; set; } = new GaitEventsDTO();
    }

    public class GaitEventsDTO
    {
        public List<string> Label { get; set; } = new List<string>();
        public List<string> Context { get; set; } = new List<string>();
        public List<string> Description { get; set; } = new List<string>();
        public List<double> Time { get; set; } = new List<double>();
    }

    public class MarkerDTO
    {
        public string Label { get; set; }
        public string UnitType { get; set; }
        public List<UnitDTO> Units { get; set; } = new List<UnitDTO>();
    }

    public class UnitDTO
    {
        public double? X { get; set; }
        public double? Y { get; set; }
        public double? Z { get; set; }
    }
}
