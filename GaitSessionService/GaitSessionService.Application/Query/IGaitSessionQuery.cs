using GaitSessionService.Application.Query.QueryDTOs;

namespace GaitSessionService.Application.Query
{
    public interface IGaitSessionQuery
    {
        Task<QueryGaitSessionDTO> GetAsync(Guid pointDataId);

        Task<List<QueryGaitSessionDTO>> GetAllAsync();
    }
}
