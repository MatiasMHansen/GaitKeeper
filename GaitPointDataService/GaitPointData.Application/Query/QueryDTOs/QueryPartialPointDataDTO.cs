using GaitPointData.Application.ValueObjectDTOs;

namespace GaitPointData.Application.Query.QueryDTOs
{
    public class QueryPartialPointDataDTO
    {   // Skal synkroniseres med 'PartialPointDataDTO' i 'DatasetService.Application.DTOs'

        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string SubjectId { get; set; }
        public double PointFreq { get; set; }
        public double Duration { get; set; }
        public int Number { get; set; }
        public List<MarkerDTO> Markers { get; set; } = new();
    }
}
