namespace GaitSessionService.Application.Command.CommandDTOs
{
    public class CreateGaitSessionDTO // Duplikeret i SharedKernel/Shared.DTOs/CreateGaitSessionDTO.cs
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

    public class BiometricsDTO
    {
        public float Height { get; set; }
        public float Weight { get; set; }
        public float LLegLength { get; set; }
        public float RLegLength { get; set; }
    }

    public class SystemInfoDTO
    {
        public string Software { get; set; }
        public string Version { get; set; }
        public string MarkerSetup { get; set; }
    }

    public class GaitAnalysisDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Context { get; set; }
        public string UnitType { get; set; }
        public float Value { get; set; }
    }

    public class GaitCycleDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Number { get; set; }
        public int StartFrame { get; set; }
        public int EndFrame { get; set; }
        public float EventStart { get; set; }
        public float EventEnd { get; set; }
        public float Duration { get; set; }
    }
}
