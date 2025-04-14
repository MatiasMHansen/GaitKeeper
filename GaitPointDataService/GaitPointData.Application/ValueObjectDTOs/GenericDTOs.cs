namespace GaitPointData.Application.ValueObjectDTOs
{
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
