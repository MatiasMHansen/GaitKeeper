using DatasetService.Domain.Entities;

namespace DatasetService.Application
{
    public interface IDatasetRepository
    {
        Task SaveAsync(Dataset dataset);
    }
}
