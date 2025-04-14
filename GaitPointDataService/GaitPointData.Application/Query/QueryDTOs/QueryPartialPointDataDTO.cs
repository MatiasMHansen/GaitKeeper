using GaitPointData.Application.ValueObjectDTOs;

namespace GaitPointData.Application.Query.QueryDTOs
{
    public class QueryPartialPointDataDTO
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string SubjectId { get; set; }
        public double PointFreq { get; set; }
        public int StartFrame { get; set; }
        public int EndFrame { get; set; }
        public int TotalFrames { get; set; }
        public List<MarkerDTO> Markers { get; set; } = new();
    }
}
