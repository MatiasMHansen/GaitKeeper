namespace GaitKeeper.WebAssembly.Models
{
    public record LabelCategories
    {
        public List<string> AngleLabels { get; set; } = new();
        public List<string> ForceLabels { get; set; } = new();
        public List<string> ModeledLabels { get; set; } = new();
        public List<string> MomentLabels { get; set; } = new();
        public List<string> PowerLabels { get; set; } = new();
        public List<string> PointLabels { get; set; } = new();
    }
}
