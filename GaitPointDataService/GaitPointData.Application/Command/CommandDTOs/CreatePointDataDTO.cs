namespace GaitPointData.Application.Command.CommandDTOs
{
    public class CreatePointDataDTO // ArrgegateRoot
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string SubjectId { get; set; }
        public double PointFreq { get; set; }
        public int StartFrame { get; set; }
        public int EndFrame { get; set; }
        public int TotalFrames { get; set; }
        public List<MarkerDTO> PointMarkers { get; set; }
        public List<MarkerDTO> AngleMarkers { get; set; }
        public List<MarkerDTO> ForceMarkers { get; set; }
        public List<MarkerDTO> ModeledMarkers { get; set; }
        public List<MarkerDTO> MomentMarkers { get; set; }
        public List<MarkerDTO> PowerMarkers { get; set; }
        public List<GaitCycleDTO> LGaitCycles { get; set; }
        public List<GaitCycleDTO> RGaitCycles { get; set; }
    }

    public class GaitCycleDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Number { get; set; }
        public double EventStart { get; set; }
        public double EventEnd { get; set; }
        public double Duration { get; set; }
        public int StartFrame { get; set; }
        public int EndFrame { get; set; }
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
