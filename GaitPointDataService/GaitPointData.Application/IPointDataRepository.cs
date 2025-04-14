using GaitPointData.Application.Query.QueryDTOs;
using GaitPointData.Domain.Aggregate;

namespace GaitPointData.Application
{
    public interface IPointDataRepository
    {
        Task SaveAsync(PointData pointData);
        Task<QueryPointDataDTO> LoadAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task EnsureBucketExistsAsync();
    }
}
