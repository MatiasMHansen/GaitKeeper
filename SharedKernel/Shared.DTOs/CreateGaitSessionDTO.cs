namespace Shared.DTOs
{
    public class CreateGaitSessionDTO // Duplikeret i Gaitsession.Application.Command.CommandDTOs
    {
        public Guid? PointDataId { get; set; }
        // Tilføjet af brugeren i UI
        public string Description { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }

        // Tilføjet i PythonC3DReader
        public string FileName { get; set; }
        public string SubjectId { get; set; }
        public float PointFreq { get; set; }
        public float AnalogFreq { get; set; }
        public int StartFrame { get; set; }
        public int EndFrame { get; set; }
        public int TotalFrames { get; set; }

        public List<string> AngleLabels { get; set; }
        public List<string> ForceLabels { get; set; }
        public List<string> ModeledLabels { get; set; }
        public List<string> MomentLabels { get; set; }
        public List<string> PowerLabels { get; set; }
        public List<string> PointLabels { get; set; }

        public BiometricsDTO Biometrics { get; set; }
        public SystemInfoDTO SystemInfo { get; set; }
        public List<GaitCycleDTO> LGaitCycles { get; set; }
        public List<GaitCycleDTO> RGaitCycles { get; set; }
        public List<GaitAnalysisDTO>? GaitAnalyses { get; set; }
    }
}
