namespace Shared.DTOs
{
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
