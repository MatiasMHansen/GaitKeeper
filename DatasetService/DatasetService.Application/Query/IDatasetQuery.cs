using DatasetService.Application.Query.QueryDTOs;

namespace DatasetService.Application.Query
{
    public interface IDatasetQuery
    {
        Task<List<QueryPreviewDatasetDTO>> GetAllAsync();
        Task<QueryDatasetDTO> GetAsync(Guid id);
    }
}
