using DatasetService.Application.Query.QueryDTOs;

public class QueryPreviewDatasetDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int NumberOfSubjects { get; set; }
    public List<string> AllLabels { get; set; } = new();
    public List<ContinuousVariableDTO> ContinuousDataSummery { get; set; } = new();
}
