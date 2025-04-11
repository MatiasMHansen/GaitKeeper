using GaitSessionService.Application.Query.QueryDTOs;

namespace GaitSessionService.Application.Query
{
    public interface IGaitSessionQuery
    {
        Task<GaitSessionQueryDTO> GetAsync(Guid pointDataId);

        Task<List<GaitSessionQueryDTO>> GetAllAsync();
    }
}
