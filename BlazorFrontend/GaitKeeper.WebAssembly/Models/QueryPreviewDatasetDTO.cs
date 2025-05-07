namespace GaitKeeper.WebAssembly.Models
{
    public class QueryPreviewDatasetDTO // kopi af: QueryPreviewDatasetDTO fra DatasetService.Application.Query.QueryDTOs
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int NumberOfSubjects { get; set; }
        public List<string> AllLabels { get; set; } = new();
        public List<ContinuousVariableDTO> ContinuousDataSummery { get; set; } = new();
    }
}
