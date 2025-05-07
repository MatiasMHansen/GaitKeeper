namespace GaitKeeper.WebAssembly.Models
{
    public class QueryDatasetDTO // kopi af: QueryPreviewDatasetDTO fra DatasetService.Application.Query.QueryDTOs
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int NumberOfSubjects { get; set; }
        public List<string> AllLabels { get; set; } = new();
        public List<QuerySubjectDTO> Subjects { get; set; } = new();
        public List<ContinuousVariableDTO> ContinuousDataSummery { get; set; } = new();
    }

    public class QuerySubjectDTO
    {
        public string SubjectId { get; set; }
        public Guid PointDataId { get; set; } // FK
        public string Description { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public float LLegLength { get; set; }
        public float RLegLength { get; set; }
    }

    public class ContinuousVariableDTO
    {
        public string Name { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double Mean { get; set; }
        public double Median { get; set; }
        public double StdDev { get; set; }
    }
}
