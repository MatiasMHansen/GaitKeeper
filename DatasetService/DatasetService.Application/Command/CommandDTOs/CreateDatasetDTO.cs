namespace DatasetService.Application.Command.CommandDTOs
{
    public class CreateDatasetDTO
    {
        public string Name { get; set; }
        public List<PartialGaitSessionDTO> gaitSessions { get; set; } = new();
    }

    public class PartialGaitSessionDTO
    {   // Skal synkroniseres med 'QueryGaitSessionDTO' i 'GaitSessionService.Application.Query.QueryDTOs'
        public string SubjectId { get; set; }
        public Guid PointDataId { get; set; }
        public string Description { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public List<string> AngleLabels { get; set; }
        public List<string> ForceLabels { get; set; }
        public List<string> ModeledLabels { get; set; }
        public List<string> MomentLabels { get; set; }
        public List<string> PowerLabels { get; set; }
        public List<string> PointLabels { get; set; }
        public BiometricsDTO Biometrics { get; set; }
    }

    public class BiometricsDTO
    {
        public float Height { get; set; }
        public float Weight { get; set; }
        public float LLegLength { get; set; }
        public float RLegLength { get; set; }
    }
}
