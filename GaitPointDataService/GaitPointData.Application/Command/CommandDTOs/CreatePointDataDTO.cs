using GaitPointData.Application.ValueObjectDTOs;

namespace GaitPointData.Application.Command.CommandDTOs
{
    public class CreatePointDataDTO // Root Aggregate
    {   // Skal synkroniseres med 'RawPointData' i 'Shared.DTOs.RawGaitData'
       
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
}
