namespace Shared.DTOs.RawGaitData
{
    public class RawGaitSessionDTO // Aggregate
    {
        public string SubjectId { get; set; }
        public BiometricsDTO Biometrics { get; set; }
        public SystemInfoDTO SystemInfo { get; set; }
    }

    public class BiometricsDTO
    {
        public float Height { get; set; }
        public float LLegLength { get; set; }
        public float RLegLength { get; set; }
        public float Weight { get; set; }
    }

    public class SystemInfoDTO
    {
        public string MarkerSetup { get; set; }
        public string Software { get; set; }
        public string Version { get; set; }
    }

}
