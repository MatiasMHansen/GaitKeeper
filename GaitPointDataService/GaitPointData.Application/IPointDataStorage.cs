using GaitPointData.Domain.Aggregate;

namespace GaitPointData.Application
{
    public interface IPointDataStorage
    {
        Task SaveAsync(PointData pointData);
        Task<PointData> LoadAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task EnsureBucketExistsAsync();
    }
}
