using GaitPointData.Application.Query.QueryDTOs;

namespace GaitPointData.Application.Query
{
    public interface IPartialPointDataQuery
    {
        Task<List<QueryPartialPointDataDTO>> GetAsync(List<Guid> ids, List<string> labels);
    }
}
