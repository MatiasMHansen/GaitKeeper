using GaitSessionService.Application.Query.QueryDTOs;

namespace GaitSessionService.Application.Query
{
    public interface IGaitSessionQuery
    {
        Task<QueryGaitSessionDTO> GetAsync(Guid pointDataId);

        Task<List<QueryGaitSessionDTO>> GetAsync(List<Guid> pointDataIds);

        Task<List<QueryGaitSessionDTO>> GetAllAsync();
    }
}
